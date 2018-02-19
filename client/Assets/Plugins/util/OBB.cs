using UnityEngine;

namespace GameUtil
{
    struct OBB
    {
        public Vector3 center;
        public Vector3 dimension;
        public Matrix4x4 matrix;

        public static OBB identity = new OBB(Vector3.zero, Vector3.zero, Matrix4x4.identity);

        public OBB(Vector3 center, Vector3 dimen, Matrix4x4 mtx)
        {
            this.center = center;
            this.dimension = dimen;
            this.matrix = mtx;
        }

        public bool Intersect(OBB obb)
        {
            return Intersect(this, obb);
        }

        public bool Intersect(Ray ray, out float dis)
        {
            return Intersect(ray, this, out dis);
        }

        //only used when you make sure the 2 obb intersect
//     public Vector3 GetIntersectPos(OBB obb)
//     {
//         return GetIntersectPos(this, obb);
//     }

        public OBB TransformOBB(Matrix4x4 mtx)
        {
            OBB obb = new OBB();
            obb.dimension = this.dimension;
            obb.center = mtx.MultiplyPoint3x4(this.center);

            mtx.SetColumn(3, new Vector4(0, 0, 0, 1));
            obb.matrix = mtx * this.matrix;

            return obb;
        }

        //linear interpolate, a * (1 - t) + b * t
        public static OBB Interpolate(OBB a, OBB b, float t)
        {
            OBB obb = new OBB();
            t = Mathf.Clamp(t, 0, 1);
            obb.center = a.center * (1 - t) + b.center * t;
            obb.dimension = a.dimension * (1 - t) + b.dimension * t;
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    obb.matrix[i, j] = a.matrix[i, j] * (1 - t) + b.matrix[i, j] * t;
                }

            return obb;
        }

        public void DebugDraw(Color col)
        {
            Vector3 src = dimension;
            Vector3 dst = dimension;
            Vector3 src_w = src;
            Vector3 dst_w = dst;

            src.x = dimension.x;
            src.y = dimension.y;
            src.z = dimension.z;
            dst.x = -dimension.x;
            dst.y = dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = dimension.x;
            src.y = dimension.y;
            src.z = dimension.z;
            dst.x = dimension.x;
            dst.y = -dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = dimension.x;
            src.y = dimension.y;
            src.z = dimension.z;
            dst.x = dimension.x;
            dst.y = dimension.y;
            dst.z = -dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = -dimension.y;
            src.z = -dimension.z;
            dst.x = dimension.x;
            dst.y = -dimension.y;
            dst.z = -dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = -dimension.y;
            src.z = -dimension.z;
            dst.x = -dimension.x;
            dst.y = dimension.y;
            dst.z = -dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = -dimension.y;
            src.z = -dimension.z;
            dst.x = -dimension.x;
            dst.y = -dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = dimension.x;
            src.y = -dimension.y;
            src.z = -dimension.z;
            dst.x = dimension.x;
            dst.y = dimension.y;
            dst.z = -dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = dimension.x;
            src.y = -dimension.y;
            src.z = -dimension.z;
            dst.x = dimension.x;
            dst.y = -dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = dimension.y;
            src.z = -dimension.z;
            dst.x = dimension.x;
            dst.y = dimension.y;
            dst.z = -dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = dimension.y;
            src.z = -dimension.z;
            dst.x = -dimension.x;
            dst.y = dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = -dimension.y;
            src.z = dimension.z;
            dst.x = -dimension.x;
            dst.y = dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);

            src.x = -dimension.x;
            src.y = -dimension.y;
            src.z = dimension.z;
            dst.x = dimension.x;
            dst.y = -dimension.y;
            dst.z = dimension.z;
            src_w = matrix.MultiplyPoint3x4(src) + center;
            dst_w = matrix.MultiplyPoint3x4(dst) + center;
            UnityEngine.Debug.DrawLine(src_w, dst_w, col);
        }

        public static bool Intersect(OBB a, OBB b)
        {
            Vector3 ac = a.center;
            Vector3 bc = b.center;

            Vector3 ae = a.dimension;
            Vector3 be = b.dimension;

            Vector3 u0 = new Vector3(a.matrix.m00, a.matrix.m10, a.matrix.m20);
            u0.Normalize();
            Vector3 u1 = new Vector3(a.matrix.m01, a.matrix.m11, a.matrix.m21);
            u1.Normalize();
            Vector3 u2 = new Vector3(a.matrix.m02, a.matrix.m12, a.matrix.m22);
            u2.Normalize();
            Vector3[] au = new Vector3[3] {u0, u1, u2};

            u0 = new Vector3(b.matrix.m00, b.matrix.m10, b.matrix.m20);
            u0.Normalize();
            u1 = new Vector3(b.matrix.m01, b.matrix.m11, b.matrix.m21);
            u1.Normalize();
            u2 = new Vector3(b.matrix.m02, b.matrix.m12, b.matrix.m22);
            u2.Normalize();
            Vector3[] bu = new Vector3[3] { u0, u1, u2 };

            float[][] R = new float[3][];
            R[0] = new float[3];
            R[1] = new float[3];
            R[2] = new float[3];

            float[][] AbsR = new float[3][];
            AbsR[0] = new float[3];
            AbsR[1] = new float[3];
            AbsR[2] = new float[3];

            float ra, rb;

            // Compute rotation matrix expressing b in a's coordinate frame
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    R[i][j] = Vector3.Dot(au[i], bu[j]);
                }

            // Compute translation vector t
            Vector3 t = bc - ac;
            // Bring translation into a's coordinate frame
            t = new Vector3(Vector3.Dot(t, au[0]), Vector3.Dot(t, au[1]), Vector3.Dot(t, au[2]));

            // Compute common subexpressions. Add in an epsilon term to
            // counteract arithmetic errors when two edges are parallel and
            // their cross product is (near) null (see text for details)
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    AbsR[i][j] = Mathf.Abs(R[i][j]) + Mathf.Epsilon;
                }

            // Test axes L = A0, L = A1, L = A2
            for (int i = 0; i < 3; i++)
            {
                ra = ae[i];
                rb = be[0] * AbsR[i][0] + be[1] * AbsR[i][1] + be[2] * AbsR[i][2];
                if (Mathf.Abs(t[i]) > ra + rb)
                {
                    return false;
                }
            }

            // Test axes L = B0, L = B1, L = B2
            for (int i = 0; i < 3; i++)
            {
                ra = ae[0] * AbsR[0][i] + ae[1] * AbsR[1][i] + ae[2] * AbsR[2][i];
                rb = be[i];
                if (Mathf.Abs(t[0] * R[0][i] + t[1] * R[1][i] + t[2] * R[2][i]) > ra + rb)
                {
                    return false;
                }
            }

            // Test axis L = A0 x B0
            ra = ae[1] * AbsR[2][0] + ae[2] * AbsR[1][0];
            rb = be[1] * AbsR[0][2] + be[2] * AbsR[0][1];
            if (Mathf.Abs(t[2] * R[1][0] - t[1] * R[2][0]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A0 x B1
            ra = ae[1] * AbsR[2][1] + ae[2] * AbsR[1][1];
            rb = be[0] * AbsR[0][2] + be[2] * AbsR[0][0];
            if (Mathf.Abs(t[2] * R[1][1] - t[1] * R[2][1]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A0 x B2
            ra = ae[1] * AbsR[2][2] + ae[2] * AbsR[1][2];
            rb = be[0] * AbsR[0][1] + be[1] * AbsR[0][0];
            if (Mathf.Abs(t[2] * R[1][2] - t[1] * R[2][2]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A1 x B0
            ra = ae[0] * AbsR[2][0] + ae[2] * AbsR[0][0];
            rb = be[1] * AbsR[1][2] + be[2] * AbsR[1][1];
            if (Mathf.Abs(t[0] * R[2][0] - t[2] * R[0][0]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A1 x B1
            ra = ae[0] * AbsR[2][1] + ae[2] * AbsR[0][1];
            rb = be[0] * AbsR[1][2] + be[2] * AbsR[1][0];
            if (Mathf.Abs(t[0] * R[2][1] - t[2] * R[0][1]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A1 x B2
            ra = ae[0] * AbsR[2][2] + ae[2] * AbsR[0][2];
            rb = be[0] * AbsR[1][1] + be[1] * AbsR[1][0];
            if (Mathf.Abs(t[0] * R[2][2] - t[2] * R[0][2]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A2 x B0
            ra = ae[0] * AbsR[1][0] + ae[1] * AbsR[0][0];
            rb = be[1] * AbsR[2][2] + be[2] * AbsR[2][1];
            if (Mathf.Abs(t[1] * R[0][0] - t[0] * R[1][0]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A2 x B1
            ra = ae[0] * AbsR[1][1] + ae[1] * AbsR[0][1];
            rb = be[0] * AbsR[2][2] + be[2] * AbsR[2][0];
            if (Mathf.Abs(t[1] * R[0][1] - t[0] * R[1][1]) > ra + rb)
            {
                return false;
            }

            // Test axis L = A2 x B2
            ra = ae[0] * AbsR[1][2] + ae[1] * AbsR[0][2];
            rb = be[0] * AbsR[2][1] + be[1] * AbsR[2][0];
            if (Mathf.Abs(t[1] * R[0][2] - t[0] * R[1][2]) > ra + rb)
            {
                return false;
            }

            // Since no separating axis found, the OBBs must be intersecting
            return true;
        }

        public static bool Intersect(Ray ray, OBB obb, out float dis)
        {
            Matrix4x4 rotM = obb.matrix;
            Vector3 dir = rotM.inverse.MultiplyVector(ray.direction);
            Matrix4x4 transM = rotM;
            transM.SetColumn(3, new Vector4(obb.center.x, obb.center.y, obb.center.z, 1));
            Vector3 ori = transM.inverse.MultiplyPoint3x4(ray.origin);

            Ray r = new Ray(ori, dir);
            Bounds bd = obb.GetLocalAABB();

            return MathUtil.RaycastBound(r, bd, out dis);
        }

        //only used when you make sure the 2 obb intersect
        //obb_r intersect obb_l
//     public static Vector3 GetIntersectPos(OBB obb_l, OBB obb_r)
//     {
//         Matrix4x4 m_l = obb_l.matrix;
//         m_l.SetColumn(3, new Vector4(obb_l.center.x, obb_l.center.y, obb_l.center.z, 1));
//
//         Matrix4x4 m_l_inv = m_l.inverse;
//
//         Matrix4x4 m_r = obb_r.matrix;
//         m_r.SetColumn(3, new Vector4(obb_r.center.x, obb_r.center.y, obb_r.center.z, 1));
//
//         Matrix4x4 mtx = m_l_inv * m_r;
//
//         Vector3 src = Vector3.zero;
//         Vector3[] dst = new Vector3[8];
//
//         src.x = obb_r.dimension.x;
//         src.y = obb_r.dimension.y;
//         src.z = obb_r.dimension.z;
//         dst[0] = mtx.MultiplyPoint3x4(src);
//
//         src.x = obb_r.dimension.x;
//         src.y = -obb_r.dimension.y;
//         src.z = obb_r.dimension.z;
//         dst[1] = mtx.MultiplyPoint3x4(src);
//
//         src.x = obb_r.dimension.x;
//         src.y = obb_r.dimension.y;
//         src.z = -obb_r.dimension.z;
//         dst[2] = mtx.MultiplyPoint3x4(src);
//
//         src.x = obb_r.dimension.x;
//         src.y = -obb_r.dimension.y;
//         src.z = -obb_r.dimension.z;
//         dst[3] = mtx.MultiplyPoint3x4(src);
//
//         src.x = -obb_r.dimension.x;
//         src.y = obb_r.dimension.y;
//         src.z = obb_r.dimension.z;
//         dst[4] = mtx.MultiplyPoint3x4(src);
//
//         src.x = -obb_r.dimension.x;
//         src.y = -obb_r.dimension.y;
//         src.z = obb_r.dimension.z;
//         dst[5] = mtx.MultiplyPoint3x4(src);
//
//         src.x = -obb_r.dimension.x;
//         src.y = obb_r.dimension.y;
//         src.z = -obb_r.dimension.z;
//         dst[6] = mtx.MultiplyPoint3x4(src);
//
//         src.x = -obb_r.dimension.x;
//         src.y = -obb_r.dimension.y;
//         src.z = -obb_r.dimension.z;
//         dst[7] = mtx.MultiplyPoint3x4(src);
//
//         Bounds bd = obb_l.GetLocalAABB();
//         //invalid
//         int idx = -1;
//         float sqrMag = -1.0f;
//         for(int i = 0; i < 8; ++i)
//         {
//             if(!MathUtil.BoundContainsPoint(bd, dst[i]))
//                 continue;
//
//             if(idx == -1)
//             {
//                 sqrMag = dst[i].sqrMagnitude;
//                 idx = i;
//             }
//             else if(dst[i].sqrMagnitude < sqrMag)
//             {
//                 sqrMag = dst[i].sqrMagnitude;
//                 idx = i;
//             }
//         }
//
// //         SysUtil.Assert(idx >= 0 && idx < 8, "illegal intersect point");
// //         App.Logger.LogMsg(idx.ToString());
//
//         if(idx < 0 || idx >= 8)
//         {
//             bool intersect = Intersect(obb_l, obb_r);
//             App.Logger.LogMsg(intersect.ToString());
// //             App.Logger.LogMsg(idx.ToString());
// //             App.Logger.LogMsg(sqrMag.ToString());
//
//             App.Logger.LogMsg(bd.min.x.ToString());
//             App.Logger.LogMsg(bd.min.y.ToString());
//             App.Logger.LogMsg(bd.min.z.ToString());
//
//             App.Logger.LogMsg(bd.max.x.ToString());
//             App.Logger.LogMsg(bd.max.y.ToString());
//             App.Logger.LogMsg(bd.max.z.ToString());
// //             int j = 0;
// //             while(j < 8)
// //             {
// //                 App.Logger.LogMsg(dst[j].ToString());
// //                 j++;
// //             }
//
//             SysUtil.DebugBreak();
//         }
//         else
//             App.Logger.LogMsg("intersect: " + idx);
//
//         return m_l.MultiplyPoint3x4(dst[idx]);
//     }

        public Bounds GetAABB()
        {
            Bounds bd = GetLocalAABB();
            Matrix4x4 mtx = matrix;
            mtx.SetColumn(3, new Vector4(center.x, center.y, center.z, 1));
            return MathUtil.TransformBounds(mtx, bd);
        }

        private Bounds GetLocalAABB()
        {
            return new Bounds(Vector3.zero, dimension * 2);
        }
    }
}