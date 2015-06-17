using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class PolygonCollider : Collider
    {
        public const int MaxVertex = 64;

        // The vertices of the polygon collider on local coordinate 
        // translated by the center of the collider
        private Vector2[] _vertices = {
                                        new Vector2(1.0f, 1.0f),
                                        new Vector2(-1.0f, 1.0f),
                                        new Vector2(-1.0f, -1.0f),
                                        new Vector2(1.0f, -1.0f)
                                    };
        public Vector2[] vertices
        {
            get { return _vertices; }
            private set
            {
                _vertices = value;
            }
        } 
        public int vertexCount
        {
            get
            {
                if (vertices == null)
                {
                    Console.WriteLine("'baseVertices' is null !");
                    return -1;
                }

                return vertices.Length;
            }
        }
        // The normal verctors of each face of the polygon collider
        public Vector2[] normals;

        /*
        // The affine matrix , including rotation using radians
        public Matrix affMat = new Matrix();
        */

        public override Vector2 center
        {
            get { return _center; }
            set 
            {
                _center = value;

                // assign new local position 
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices[i] += center;
                }
            }
        }

        /*
        public override void SetOrient()
        {
            // transformation vertices , rotate -> translate
            affMat = Matrix.CreateRotationZ(MathHelper.ToRadians(gameObject.rotation));
            
        }*/

        /// <summary>
        /// Create box collider 
        /// </summary>
        public void SetBox(float halfWidth, float halfHeight)
        {
            vertices = new Vector2[4];
            normals = new Vector2[4];

            vertices[0] = new Vector2(-halfWidth, -halfHeight) + center;
            vertices[1] = new Vector2(halfWidth, -halfHeight) + center;
            vertices[2] = new Vector2(halfWidth, halfHeight) + center;
            vertices[3] = new Vector2(-halfWidth, halfHeight) + center;
            normals[0] = new Vector2(0f, -1f);
            normals[1] = new Vector2(1f, 0f);
            normals[2] = new Vector2(0f, 1f);
            normals[3] = new Vector2(-1f, 0f);

        }

        /// <summary>
        /// Set verties and calculate for normal left
        /// </summary>
        public void Set(Vector2[] poly)
        {
            vertices = poly;
            normals = new Vector2[poly.Length];

            // move vertices to local-coordinate
            for (int i = 0; i < vertexCount; i++)
                vertices[i] += center;

            // Compute face normals
            for (int i = 0; i < vertexCount; i++)
            {
                // get normal vector
                Vector2 face = new Vector2(
                        vertices[(i + 1) % vertexCount].X - vertices[i].X,
                        vertices[(i + 1) % vertexCount].Y - vertices[i].Y
                    );

                // ensure ! no zero-length edges, because that's bad
                Debug.Assert(face.LengthSquared() > Mathf.Sqr(Mathf.EPSILON));

                // left normalize
                normals[i] = new Vector2(-face.Y,face.X);
                normals[i].Normalize();
            }
        }

        /// <summary>
        /// Set unarraged vertices 
        /// </summary>
        public void SetVertices(Vector2[] poly)
        {
            // No hulls with less than 3 vertices (ensure actual polygon)
            Debug.Assert(poly.Length > 2 && poly.Length <= MaxVertex);
            
            // Find the right most point on the hull
            int rightMost = 0;
            float highestXCoord = poly[0].X;

            for (int i = 1; i < poly.Length; ++i)
            {
                float x = poly[i].X;
                if (x > highestXCoord)
                {
                    highestXCoord = x;
                    rightMost = i;
                }
                // If matching x then take farthest negative y
                else if (x == highestXCoord)
                {
                    if (poly[i].Y > poly[rightMost].Y)
                        rightMost = i;
                }
            }

            int[] hull = new int[MaxVertex];
            int outCount = 0;
            int indexHull = rightMost;

            while (true)
            {
                hull[outCount] = indexHull;

                // Search for next index that wraps around the hull
                // by computing cross products to find the most counter-clockwise
                // vertex in the set, given the previos hull index
                int nextHullIndex = 0;
                for (int i = 1; i < poly.Length; ++i)
                {
                    // Skip if same coordinate as we need three unique
                    // points in the set to perform a cross product
                    if (nextHullIndex == indexHull)
                    {
                        nextHullIndex = i;
                        continue;
                    }

                    // Cross every set of three unique vertices
                    // Record each counter clockwise third vertex and add
                    // to the output hull
                    // See : http://www.oocities.org/pcgpe/math2d.html
                    Vector2 e1 = poly[nextHullIndex] - poly[hull[outCount]];
                    Vector2 e2 = poly[i] - poly[hull[outCount]];
                    float c = Mathf.Cross(e1, e2);
                    if (c < 0.0f)
                        nextHullIndex = i;

                    // Cross product is zero then e vectors are on same line
                    // therefor want to record vertex farthest along that line
                    if (c == 0.0f && e2.LengthSquared() > e1.LengthSquared())
                        nextHullIndex = i;
                }

                ++outCount;
                indexHull = nextHullIndex;

                // Conclude algorithm upon wrap-around
                if (nextHullIndex == rightMost)
                {
                    vertices = new Vector2[outCount];
                    break;
                }
              
            }

            // Copy vertices into shape's vertices
            for (int i = 0; i < vertexCount; ++i)
            {
                // Copy vertices into shape's vertices
                vertices[i] = poly[hull[i]];
                // move to center 
                //vertices[i] += center;
            }

            // Compute face normals
            normals = new Vector2[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                // get normal vector
                Vector2 face = vertices[(i + 1) % vertexCount] - vertices[i];
               
                // ensure ! no zero-length edges, because that's bad
                Debug.Assert(face.LengthSquared() > Mathf.Sqr(Mathf.EPSILON));

                // left normalize
                normals[i] = new Vector2(face.Y, -face.X);
                normals[i].Normalize();
            }

        }

        /// <summary>
        /// The extreme point along a direction within a polygon
        /// </summary>
        public Vector2 GetSupport(Vector2 dir)
        {
            float bestProjection = -float.MaxValue;
            Vector2 bestVertex = Vector2.Zero;

            for (int i = 0; i < vertexCount; ++i)
            {
                Vector2 v = vertices[i];
                float projection = Vector2.Dot(v, dir);

                if (projection > bestProjection)
                {
                    bestVertex = v;
                    bestProjection = projection;
                }
            }

            return bestVertex; 
        }

    } 
}
