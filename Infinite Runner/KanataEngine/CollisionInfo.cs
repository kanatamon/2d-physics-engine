using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infinite_Runner.KanataEngine
{
    class CollisionInfo
    {
        Collider A;
        Collider B;

        public float penetration;
        public Vector2 normal;
        public Vector2[] contacts = new Vector2[2];
        public int contactCount;
        public float restitution;
        public float dynamicFriction;
        public float staticFriction;

        public CollisionInfo(Collider a, Collider b)
        {
            A = a;
            B = b;
        }

        public void Solve()
        {
            // Call collision
            Physics2D.Dispath(this, A, B);
        }

        public void Initialize(GameTime gametime)
        {
            // Calculate average restitution
            restitution = Math.Min(A.rigidbody.restitution, B.rigidbody.restitution);

            // Calculate static and dynamic friction
            staticFriction = (float)Math.Sqrt(A.rigidbody.staticFriction * A.rigidbody.staticFriction);
            dynamicFriction = (float)Math.Sqrt(A.rigidbody.dynamicFriction * A.rigidbody.dynamicFriction);

            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                Vector2 ra = contacts[i] - A.gameObject.position;
                Vector2 rb = contacts[i] - B.gameObject.position;

                Vector2 rv =
                    B.rigidbody.velocity + Mathf.Cross(B.rigidbody.angularVelocity, rb) -
                    A.rigidbody.velocity - Mathf.Cross(A.rigidbody.angularVelocity, ra);

                // Determine if we should perform a resting collision or not
                // The idea is if the only thing moving this object is gravity,
                // then the collision should be performed without any restitution
                if (rv.LengthSquared() < ((float)gametime.ElapsedGameTime.TotalSeconds * A.rigidbody.gravity).LengthSquared() + Mathf.EPSILON)
                    restitution = 0.0f;

            }
        }
      
        public void ApplyImpulse()
        {
            if (Mathf.Equal(A.rigidbody.invMass + B.rigidbody.invMass, 0.0f))
            {
                InfiniteMassCorrection();
                return;
            }

            for (int i = 0; i < contactCount; ++i)
            {
                // Calculate radii from COM to contact
                Vector2 ra = contacts[i] - A.gameObject.position;
                Vector2 rb = contacts[i] - B.gameObject.position;

                // Relative velocity 
                Vector2 rv =
                    B.rigidbody.velocity + Mathf.Cross(B.rigidbody.angularVelocity, rb) -
                    A.rigidbody.velocity - Mathf.Cross(A.rigidbody.angularVelocity, ra);

                // Relative velocity along the normal
                float contactVel = Vector2.Dot(rv, normal);

                // Do not resolve if velocities are separating
                if (contactVel > 0)
                    return;

                float raCrossN = Mathf.Cross(ra, normal);
                float rbCrossN = Mathf.Cross(rb, normal);
                float invMassSum = A.rigidbody.invMass + B.rigidbody.invMass + 
                    Mathf.Sqr(raCrossN) * A.rigidbody.invInertia + 
                    Mathf.Sqr(rbCrossN) * B.rigidbody.invInertia;

                // Calculate impulse scalar
                float j = -(1.0f + restitution) * contactVel;
                j /= invMassSum;
                j /= (float)contactCount;

                // Apply impulse
                Vector2 impulse = normal * j;
                A.rigidbody.AddImpulse(-impulse, ra);
                B.rigidbody.AddImpulse(impulse, rb);

                // Friction impulse
                rv = B.rigidbody.velocity + Mathf.Cross(B.rigidbody.angularVelocity, rb) -
                    A.rigidbody.velocity - Mathf.Cross(A.rigidbody.angularVelocity, ra);

                Vector2 t = rv - (normal * Vector2.Dot(rv, normal));
                
                // district NaN if t = zero's vector then let it be zero
                if (t != Vector2.Zero)
                    t.Normalize();
                
                // j tangent magnitude
                float jt = -Vector2.Dot(rv, t);
                jt /= invMassSum;
                jt /= (float)contactCount;

                // Don't apply tiny friction impulses
                if (Mathf.Equal(jt, 0.0f))
                    return;

                // Coulumb's law
                Vector2 tangentImpulse;
                if (Math.Abs(jt) < j * staticFriction)
                    tangentImpulse = t * jt;
                else
                    tangentImpulse = -j * t * dynamicFriction;

                // Apply friction impulse
                A.rigidbody.AddImpulse(-tangentImpulse, ra);
                B.rigidbody.AddImpulse(tangentImpulse, rb);

            }
        }

        public void PositionCorrection()
        {
            const float k_slop = 0.05f; // penetration allowance
            const float percent = 0.4f; // penetration percentage correct
            Vector2 correction = ( Math.Max(penetration - k_slop, 0.0f) / (A.rigidbody.invMass + B.rigidbody.invMass))* normal * percent;
            A.gameObject.position -= correction * A.rigidbody.invMass;
            B.gameObject.position += correction * B.rigidbody.invMass;
        }
        
        public void InfiniteMassCorrection()
        {
            A.rigidbody.velocity = Vector2.Zero;
            B.rigidbody.velocity = Vector2.Zero;
        }

    }
}
