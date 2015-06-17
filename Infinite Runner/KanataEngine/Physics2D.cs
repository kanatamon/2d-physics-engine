using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    static class Physics2D
    {
        #region Physics2D value

        public const float gravityForce = 9.81f;
        public const float timeStepSqrt = 0.0002777f; // Fixed timeStep^2
        public const float unit = 100f;    // Meature of 1pixel : 1cent
        public const float unitSqrt = 10000f; // Meature of 1p : 1c square
        public const float dealtaTime = 0.13f;
        
        #endregion
        // The gravity force 
        public static Vector2 gravity = new Vector2(0, 9.81f);

        // The scene that using the Physics2D .
        public static Scene scene = null;

        // The privot point of every collider .
        public static Vector2 colliderOrigin = Vector2.Zero;

        #region Physics2D texture

        // The texture for classifying collision collider .
        public static Texture2D colliderTexture;
        // The texture for classifying trigger collider .
        public static Texture2D triggerTexture;
        public static Texture2D lineTexture;

        #endregion

        static List<GameObject> gameColliders = new List<GameObject>();
        static List<Rigidbody2D> gameRigidbodies = new List<Rigidbody2D>();

        #region Initialization

        /// <summary>
        /// Load all content that this class need to use 
        /// this must call before the other 'LoadContent' 
        /// </summary>
        /// <param name="contentManager">Reference to Game.Content</param>
        public static void LoadContent(GraphicsDevice graphicsDevice,ContentManager contentManager)
        {
            // Load the texture , used for all collider   
            colliderTexture = contentManager.Load<Texture2D>("green_tile");
            triggerTexture = contentManager.Load<Texture2D>("red_tile");

            lineTexture = new Texture2D(graphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] { Color.White });

            // Set origin at the center of colliderTexture
            // the origin make the native position of any GameObject at the center 
            colliderOrigin = new Vector2((float)Physics2D.colliderTexture.Width / 2, (float)Physics2D.colliderTexture.Height / 2);

        }

        #endregion

        #region Update
        /// <summary>
        /// Update the Physics2D
        /// </summary>
        /// <param name="gametime"></param>
        public static void Update(GameTime gametime)
        {
            InitUpdate();
            // detect collision event(s)
            //DetectCollisionEvent();
            Step(gametime);

        }

        private static void InitUpdate()
        {
            // clear lists
            gameRigidbodies.Clear();
            gameColliders.Clear();
            //scene.GameObjects.
            foreach (GameObject gameObject in scene.GameObjects)
            {
                /*
                // Check if there is rigidbody in gameObject
                if (gameObject.rigidbody != null)
                    gameRigidbodies.Add(gameObject.rigidbody);
                */
                // Check if there is any collider in gameObject
                if (gameObject.colliders.Count > 0 && gameObject.GetComponent<Rigidbody2D>() != null) 
                    gameColliders.Add(gameObject);
          
            }
        }

        /// <summary>
        /// Draw colliders.
        /// </summary>
        public static void DrawColliders2(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        Camera.main.transformation);

            // Draw mouse 
            DrawCircle(spriteBatch, Camera.main.ScreenToWorld(Input.mousePosition), 50.0f);
            
            // Draw each collider in the world
            foreach (GameObject gameObj in gameColliders)
            {
                // Draw a point representing the gameObject in the game world.
                DrawCircle(spriteBatch, gameObj.position, 20f);

                // Creats a transformation matrix traslating to 'world coordinate'
                Matrix toWorldMat = Matrix.CreateRotationZ(MathHelper.ToRadians(gameObj.rotation))
                    * Matrix.CreateTranslation(gameObj.position.X, gameObj.position.Y, 0f);
                
                foreach (Collider collider in gameObj.colliders)
                {
                    // calculate world coordinate from local coordinate 
                    Vector2 centerOnWorld = Vector2.Transform(collider.center, toWorldMat);

                    // Create a tranfomation matrix traslating to 'local coordinate' 
                    Matrix toLocalMat = Matrix.CreateTranslation(collider.center.X, collider.center.Y, 0f);
                    
                    // check the type of collider 
                    if (collider.GetType() == typeof(CircleCollider))
                    {
                        // Cast the collider as circle collider
                        CircleCollider circle = (CircleCollider)collider;

                        // M(degree) notation 
                        Vector2 mNotation = Vector2.Transform(new Vector2(circle.radius, 0f), toLocalMat * toWorldMat);

                        // Draw the circle 
                        DrawCircle(spriteBatch, centerOnWorld, circle.radius);
                   
                        // Draw a radius line that along with 0 degree 
                        DrawLine(spriteBatch, centerOnWorld, mNotation);
                    }
                    else
                    {
                        // Cast the collider as the polygon collider 
                        PolygonCollider poly = (PolygonCollider)collider;

                        for (int i = 0; i < poly.vertexCount; i++) 
                        {
                            // Translate the local vertise of the polygon to world
                            Vector2 start = Vector2.Transform(poly.vertices[i], toLocalMat * toWorldMat);
                            Vector2 end = Vector2.Transform(poly.vertices[(i + 1) % poly.vertexCount], toLocalMat * toWorldMat);
                            
                            // Draw a surface of polygon 
                            DrawLine(spriteBatch, start, end);
                        }                      
                    } 
                }
            }

            spriteBatch.End();

        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(lineTexture,
                new Rectangle(          // rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //spriteBatch will strech the texture to fill this rectangle
                    2),                 //width of line, change this to make thicker line
                null,
                Color.Green,              //colour of line
                angle,                  //angle of line (calulated above)
                Vector2.Zero,      // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawCircle(SpriteBatch spritbatch, Vector2 center, float radius)
        {
            // Initialize base value.
            const int nSlice = 36;
            float rotation = (float)(Math.PI * 2 / nSlice);
            float angle = rotation;

            // Initialize begining point.
            float x = (float)(radius * Math.Cos(angle)) + center.X;
            float y = (float)(radius * Math.Sin(angle)) + center.Y;
            Vector2 start = new Vector2(center.X + radius, center.Y);
            Vector2 end = new Vector2(x, y);

            // Draw circle
            for (int i = 0; i < nSlice; i++)
            {
                DrawLine(spritbatch, start, end);

                angle += rotation;
                x = (float)(radius * Math.Cos(angle)) + center.X;
                y = (float)(radius * Math.Sin(angle)) + center.Y;

                start = end;
                end = new Vector2(x, y);

            }
        }

        #endregion
       
        #region New! Physics collision engine
        public static List<CollisionInfo> contacts = new List<CollisionInfo>();

        public static void Dispath(CollisionInfo info, Collider A, Collider B)
        {
            // Dispath collision
            if (A.GetType() == typeof(PolygonCollider))
            {
                if (B.GetType() == typeof(PolygonCollider))
                    Physics2D.Polygon2Polygon(info, (PolygonCollider)A, (PolygonCollider)B);
                else
                    Physics2D.Polygon2Circle(info, (PolygonCollider)A, (CircleCollider)B);
            }
            else
            {
                if (B.GetType() == typeof(CircleCollider))
                    Physics2D.Circle2Circle(info, (CircleCollider)A, (CircleCollider)B);
                else
                    Physics2D.Circle2Polygon(info, (CircleCollider)A, (PolygonCollider)B);
            }
        }

        /// <summary>
        /// Apply physics to current world.
        /// </summary>
        /// <param name="gameTime"></param>
        static void Step(GameTime gameTime)
        {
            // Generate new collision info
            contacts.Clear();

            for (int i = 0; i < gameColliders.Count; ++i)
            {   
                for (int j = i + 1; j < gameColliders.Count; ++j)
                {
                    // Reject collision if the colliders are infinite.  
                    if (gameColliders[i].GetComponent<Rigidbody2D>().invMass == 0.0f
                        && gameColliders[j].GetComponent<Rigidbody2D>().invMass == 0.0f)
                        continue;
                    
                    foreach (Collider a in gameColliders[i].GetComponents<Collider>())
                    {
                        foreach (Collider b in gameColliders[j].GetComponents<C>)
                        {
                            CollisionInfo info = new CollisionInfo(a, b);
                            info.Solve();

                            if (info.contactCount > 0)
                                contacts.Add(info);
                        }
                    }

                }
            }

            // Integrate forces.
            for (int i = 0; i < gameColliders.Count; ++i)
                IntegateForces(gameColliders[i].GetComponent<Rigidbody2D>(), gameTime);
            
            // Initalize collision.
            for (int i = 0; i < contacts.Count; ++i)
                contacts[i].Initialize(gameTime);

            // Solve collisions by n iteration.
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < contacts.Count; ++j)
                    contacts[j].ApplyImpulse();
   
            // Integrate velocities.
            for (int i = 0; i < gameColliders.Count; ++i)
                IntegateVelocity(gameColliders[i].GetComponent<Rigidbody2D>(), gameTime);
            
            // Correct positions.
            for (int i = 0; i < contacts.Count; ++i)
                contacts[i].PositionCorrection();

            // Clear all forces.
            // Integrate velocities
            for (int i = 0; i < gameColliders.Count; ++i)
            {
                //Point scrPos = Camera.main.WorldToScreen(gameColliders[i].position);
                gameColliders[i].GetComponent<Rigidbody2D>().force = Vector2.Zero;
                gameColliders[i].GetComponent<Rigidbody2D>().torque = 0.0f;

            }
           // Console.WriteLine("Total : "+gameColliders.Count);
        }

        static void IntegateVelocity(Rigidbody2D rb, GameTime gameTime)
        {
            if (rb.invMass == 0.0f) return;

            rb.gameObject.position += rb.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //b.orient += b.angularVelocity * (float)gameTime.ElapsedGameTime.TotalMinutes;
            rb.gameObject.rotation += MathHelper.ToDegrees(rb.angularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // apply rotation to collider
            foreach (Collider collider in rb.gameObject.colliders)
                collider.SetOrient();
    
            IntegateForces(rb, gameTime);
        }

        static void IntegateForces(Rigidbody2D rb, GameTime gameTime)
        {
            if (rb.invMass == 0.0f) return;
            
            rb.velocity += (rb.force * rb.invMass + rb.gravity) * (float)gameTime.ElapsedGameTime.TotalSeconds / 2.0f;
            rb.angularVelocity += rb.torque * rb.invInertia * (float)gameTime.ElapsedGameTime.TotalSeconds / 2.0f;
        }

        static float FindAxisLeastPenetration(ref int faceIndex, PolygonCollider A, PolygonCollider B)
        {
            float bestDistance = -float.MaxValue;
            int bestIndex = 0;

            for (int i = 0; i < A.vertexCount; ++i)
            {
                // Retrieve a face normal from A
                Vector2 normal = A.normals[i];
                Vector2 normalAff = Vector2.Transform(normal, A.affMat);

                // Transform face normal into B's model space
                Matrix buT = Matrix.Transpose(B.affMat);
                normal = Vector2.Transform(normalAff, buT);

                // Retrieve support point from B along -n
                Vector2 s = B.GetSupport(-normal);

                // Retrieve vertex on face from A, transform into
                // B's model space
                Vector2 v = A.vertices[i];
                
                //v = Vector2.Transform(v, A.affMat) + A.worldPosition;
                v = Vector2.Transform(v + A.center, A.affMat) + A.position;
                v -= Vector2.Transform(B.center, B.affMat) + B.position;
                v = Vector2.Transform(v, buT);

                // Compute penetration ditance (in B's model space)
                float depth = Vector2.Dot(normal, s - v);

                // Store greatest distance
                if (depth > bestDistance)
                {
                    bestDistance = depth;
                    bestIndex = i;
                }
            }

            faceIndex = bestIndex;

            return bestDistance;
        }

        static void FindIncidentFace(ref Vector2[] v, PolygonCollider refPoly, PolygonCollider incPoly, int refIndex)
        {
            Vector2 refNormal = refPoly.normals[refIndex];

            // Calculate normal in incident' frame of reference
            refNormal = Vector2.Transform(refNormal, refPoly.affMat); // to world space
            refNormal = Vector2.Transform(refNormal, Matrix.Transpose(incPoly.affMat)); // to incident's model space
            
            // Find most anti-normal face on incident polygon
            int incidentFace = 0;
            float minDot = float.MaxValue;

            for (int i = 0; i < incPoly.vertexCount; ++i)
            {
                float dot = Vector2.Dot(refNormal, incPoly.normals[i]);

                if (dot < minDot)
                {
                    minDot = dot;
                    incidentFace = i;
                }
            }

            // Assign face vertices for incidentFace
            v[0] = Vector2.Transform(incPoly.vertices[incidentFace] + incPoly.center, incPoly.affMat) + incPoly.position;
            incidentFace = incidentFace + 1 >= incPoly.vertexCount ? 0 : incidentFace + 1;
            v[1] = Vector2.Transform(incPoly.vertices[incidentFace] + incPoly.center, incPoly.affMat) + incPoly.position;
        }


        static int Clip(Vector2 n, float c, ref Vector2[] face)
        {
            int sp = 0;
            Vector2[] outVectors = { face[0], face[1] };

            // REtrieve distances from each endpoint to the line
            // d = ax + by - c
            float d1 = Vector2.Dot(n, face[0]) - c;
            float d2 = Vector2.Dot(n, face[1]) - c;

            // If negative (beghind plane) clip
            if (d1 <= 0.0f) outVectors[sp++] = face[0];
            if (d2 <= 0.0f) outVectors[sp++] = face[1];

            // If the points are on differnt sides of the plane
            if (d1 * d2 < 0.0f) // less than to ignore - 0.0f
            {
                // Push intersection point
                float alpha = d1 / (d1 - d2);
                outVectors[sp] = face[0] + alpha * (face[1] - face[0]);
                ++sp;
            }

            // Assign our new converted values
            face[0] = outVectors[0];
            face[1] = outVectors[1];

            Debug.Assert(sp != 3);
            
            return sp;
        }

        // Make a collision info between collider A and B.
        static void Polygon2Polygon(CollisionInfo info, PolygonCollider A, PolygonCollider B) 
        {
            info.contactCount = 0;

            // Check for a separating axis with A's face planes
            int faceA = 0;
            float penetrationA = FindAxisLeastPenetration(ref faceA, A, B);
            if (penetrationA >= 0.0f)
                return;

            // Check for a separating axis with B's face planes
            int faceB = 0;
            float penetrationB = FindAxisLeastPenetration(ref faceB, B, A);
            if (penetrationB >= 0.0f)
                return;

            int refIndex;
            bool flip;  // Always point from a to b

            PolygonCollider refPoly;    // reference
            PolygonCollider incPoly;    // incident

            // Determine which shape contains reference face
            if (Mathf.BiasGreaterThan(penetrationA, penetrationB))
            {
                refPoly = A;
                incPoly = B;
                refIndex = faceA;
                flip = false;
            }
            else
            {
                refPoly = B;
                incPoly = A;
                refIndex = faceB;
                flip = true;
            }

            // World space incident face
            Vector2[] incidentFace = new Vector2[2];
            FindIncidentFace(ref incidentFace, refPoly, incPoly, refIndex);

            // Setup reference face vertices
            Vector2 v1 = refPoly.vertices[refIndex];
            refIndex = refIndex + 1 == refPoly.vertexCount ? 0 : refIndex + 1;
            Vector2 v2 = refPoly.vertices[refIndex];

            // Transform vertices to world space
            /*
            v1 = Vector2.Transform(v1, refPoly.affMat) + refPoly.worldPosition;
            v2 = Vector2.Transform(v2, refPoly.affMat) + refPoly.worldPosition;
            */
            // ***แก้ล่าสุด วันที่ 5/30/2015 : ยังไม่เสร็จ พัฒนามาจาก comment ข้างบนนี้
            /*
            v1 = Vector2.Transform(Vector2.Transform(v1, refPoly.affMat) + refPoly.center, refPoly.affMat) + refPoly.position;
            v2 = Vector2.Transform(Vector2.Transform(v2, refPoly.affMat) + refPoly.center, refPoly.affMat) + refPoly.position;
            */
            v1 = Vector2.Transform(v1 + refPoly.center, refPoly.affMat) + refPoly.position;
            v2 = Vector2.Transform(v2 + refPoly.center, refPoly.affMat) + refPoly.position;

            // Calculate reference face side normal in world space
            Vector2 sidePlaneNormal = v2 - v1;
            sidePlaneNormal.Normalize();

            // Orthogonalize
            Vector2 refFaceNormal = new Vector2(sidePlaneNormal.Y, -sidePlaneNormal.X);

            // ax + by = c
            // c is distance from origin
            float refC = Vector2.Dot(refFaceNormal, v1);
            float negSide = -Vector2.Dot(sidePlaneNormal, v1);
            float posSide = Vector2.Dot(sidePlaneNormal, v2);

            // Clip incident face to refernce face side planes
            if (Clip(-sidePlaneNormal, negSide, ref incidentFace) < 2)
                return; // Due to floating point error, possible to not have required points

            if (Clip(sidePlaneNormal, posSide, ref incidentFace) < 2)
                return; // Due to floating point error, possible to not have required points

            // flip
            info.normal = flip ? -refFaceNormal : refFaceNormal;

            // Keep points behind reference face
            int cp = 0;
            float separation = Vector2.Dot(refFaceNormal, incidentFace[0]) - refC;
            if (separation <= 0.0f)
            {
                info.contacts[cp] = incidentFace[0];
                info.penetration = -separation;
                ++cp;
            }
            else
            {
                info.penetration = 0.0f;
            }

            separation = Vector2.Dot(refFaceNormal, incidentFace[1]) - refC;
            if (separation <= 0.0f)
            {
                info.contacts[cp] = incidentFace[1];
                info.penetration += -separation;
                ++cp;

                // Average penetration
                info.penetration /= (float)cp;
            }

            info.contactCount = cp;

        }

        // Make a collision info between collider A and B.
        static void Circle2Polygon(CollisionInfo info, CircleCollider A, PolygonCollider B) 
        {
            info.contactCount = 0;

            // Transform circle center to Polygon model space
            Vector2 center = Vector2.Transform(A.positionOnWorld - B.positionOnWorld, Matrix.Transpose(B.affMat));
            
            // Find edge with minimum penetration
            // Exact concept as using support point in Polygon vs Polygon
            float separation = -float.MaxValue;
            int faceNormal = 0;
            for (int i = 0; i < B.vertexCount; ++i)
            {
                //Vector2 bVertex = Vector2.Transform(B.vertices[i] + B.center, B.affMat) + B.position;
                float s = Vector2.Dot(B.normals[i], center - B.vertices[i]);

                if (s > A.radius) return;

                if (s > separation)
                {
                    separation = s;
                    faceNormal = i;
                }
            }

            // Grab face's vertices
            Vector2 v1 = B.vertices[faceNormal];
            int i2 = faceNormal + 1 < B.vertexCount ? faceNormal + 1 : 0;
            Vector2 v2 = B.vertices[i2];

            // Check to see if center is within polygon
            if (separation < Mathf.EPSILON)
            {
                info.contactCount = 1;
                info.normal = -Vector2.Transform(B.normals[faceNormal], B.affMat);
                info.contacts[0] = info.normal * A.radius + A.positionOnWorld;
                info.penetration = A.radius;
                return;
            }

            // Determine which voronoi region of the edge center of circle lies within 
            float dot1 = Vector2.Dot(center - v1, v2 - v1);
            float dot2 = Vector2.Dot(center - v2, v1 - v2);
            info.penetration = A.radius - separation;

            // Closest to v1
            if (dot1 <= 0.0f)
            {
                if (Mathf.DistSqr(center, v1) > A.radius * A.radius) return;

                info.contactCount = 1;
                Vector2 n = v1 - center;
                n = Vector2.Transform(n, B.affMat);
                n.Normalize();
                info.normal = n;
                v1 = Vector2.Transform(v1, B.affMat) + B.positionOnWorld;
                info.contacts[0] = v1;
            }
            // Closest to v2
            else if (dot2 <= 0.0f)
            {
                if (Mathf.DistSqr(center, v2) > A.radius * A.radius) return;

                info.contactCount = 1;
                Vector2 n = v2 - center;
                v2 = Vector2.Transform(v2, B.affMat) + B.positionOnWorld;
                info.contacts[0] = v2;
                n = Vector2.Transform(n, B.affMat);
                n.Normalize();
                info.normal = n;
            }
            // CLosest to face
            else
            {
                Vector2 n = B.normals[faceNormal];
                if (Vector2.Dot(center - v1, n) > A.radius) return;

                n = Vector2.Transform(n, B.affMat);
                info.normal = -n;
                info.contacts[0] = info.normal * A.radius + A.positionOnWorld;
                info.contactCount = 1;
            }

        }
        
        // Make a collision info between collider A and B.
        static void Polygon2Circle(CollisionInfo info, PolygonCollider A, CircleCollider B) 
        {
            Circle2Polygon(info, B, A);
            info.normal = -info.normal;
        }

        // Make a collision info between collider A and B.
        static void Circle2Circle(CollisionInfo info, CircleCollider A, CircleCollider B) 
        {
            // Calculate translational vector, which is normal
            Vector2 normal = A.positionOnWorld - B.positionOnWorld;

            float distSqr = normal.LengthSquared();
            float radius = A.radius + B.radius;

            // Not in contact
            if (distSqr >= radius * radius)
            {
                info.contactCount = 0;
                return;
            }

            float distance = (float)Math.Sqrt(distSqr);
            info.contactCount = 1;

            if (distance == 0.0f)
            {
                info.penetration = A.radius;
                info.normal = new Vector2(1.0f, 0.0f);
                info.contacts[0] = A.positionOnWorld;
            }
            else
            {
                info.penetration = radius - distance;
                info.normal = normal / distance;    // faster than using normalized sinze we already performed sqrt
                info.contacts[0] = info.normal * A.radius + A.positionOnWorld;
            }

        }

        #endregion


        #region Overlaping Check 
        
        /// <summary>
        /// Check if point is within any collider of gameObj , return true 
        /// </summary>
        public static bool OverlapPoint(PolygonCollider collider, Vector2 point, float radius)
        {
            // prepere vector 
            Vector2 v;
            //Vector2 circleCenter = new Vector2((float)point.X, (float)point.Y);
            //Vector2 polyCenter = collider.worldPosition;
            Vector2 collPos = Vector2.Transform(collider.center, collider.affMat) + collider.position;

            Vector2 box2circle = point - collPos;
            Vector2 box2circle_normalised = Vector2.Normalize(box2circle);
            float magnitude = box2circle.Length();
            
            float separation = -float.MaxValue;

            // prepare vertex vector on rotation
            Vector2[] polyCorner = new Vector2[collider.vertexCount];
            Vector2.Transform(collider.vertices, ref collider.affMat, polyCorner);
            
            // get the maximum 
            for (int i = 0; i < collider.vertexCount; ++i)
            {
                v = polyCorner[i] - collider.center;
                float currentProj = Vector2.Dot(box2circle_normalised, v);
                
                if (currentProj > separation) separation = currentProj;
            }
            
            // check if there is overlapping
            if (magnitude > 0f && magnitude - separation - radius > 0f)
                return false;

            return true;
        }

        /*
        public static bool OverlapPoint(CircleCollider collider, Point point, float radius)
        {
            return false;
        }*/

        /// <summary>
        /// Check if the circle touching the gameObject. 
        /// </summary>
        public static bool OverlapGameObject(GameObject gameObject, Point point, float radius)
        {
            return false;
        }

        #endregion
    }
}
