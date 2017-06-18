using System;

namespace Giu.Basic.Math {
    [Serializable]
    public struct GVector2 {
        public float x;
        public float y;

        public GVector2(float v) { x = v; y = v; }
        public GVector2(float _x, float _y) { x = _x; y = _y; }
        public GVector2(GVector2 origin) { x = origin.x; y = origin.y; }

        public override bool Equals(object obj) { return base.Equals(obj); } 
        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(GVector2 v1, GVector2 v2) {
            if ((object)v1 == null || (object)v2 == null) return false;
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(GVector2 v1, GVector2 v2) {
            return !(v1 == v2);
        }

        #region Static Value

        public static readonly GVector2 Zero = new GVector2(0, 0);
        public static readonly GVector2 One = new GVector2(1, 1);
        public static readonly GVector2 UnitX = new GVector2(1, 0);
        public static readonly GVector2 UnitY = new GVector2(0, 1);

        
        public float this[int axis] {
            get { return axis == 0 ? x : axis == 1 ? y : -999999; }
            set { switch (axis) { case 0: x = value; break; case 1: y = value; break; } }
        }

        #endregion

        #region Arithmetic

        public float magnitude { get { return GMath.Sqrt(x * x + y * y); } }

        public GVector2 normal {
            get {
                float length = magnitude;
                if (length > GMath.EPS) return new GVector2(x / length, y / length);
                return Zero;
            }
        }

        public static GVector2 Add(GVector2 v1, GVector2 v2) { return new GVector2(v1.x + v2.x, v1.y + v2.y); }

        public static GVector2 operator +(GVector2 v1, GVector2 v2) { return Add(v1, v2); }

        public static GVector2 Subtract(GVector2 v1, GVector2 v2) { return new GVector2(v1.x - v2.x, v1.y - v2.y); }

        public static GVector2 operator -(GVector2 v1, GVector2 v2) { return Subtract(v1, v2); }

        public static GVector2 Negate(GVector2 v1) { return new GVector2(-v1.x, -v1.y); }

        public static GVector2 operator -(GVector2 v1) { return Negate(v1); }

        public static GVector2 Multiply(GVector2 v1, float factor) { return new GVector2(v1.x * factor, v1.y * factor); }

        public static GVector2 Multiply(GVector2 v1, GVector2 v2) { return new GVector2(v1.x * v2.x, v1.y * v2.y); }

        public static GVector2 operator *(GVector2 v1, float factor) { return Multiply(v1, factor); }

        public static GVector2 operator *(float factor, GVector2 v1) { return Multiply(v1, factor); }

        public static GVector2 operator *(GVector2 v1, GVector2 v2) { return Multiply(v1, v2); }

        public static GVector2 Divide(GVector2 v1, float divider) { if (GMath.Abs(divider) > GMath.EPS) return new GVector2(v1.x / divider, v1.y / divider); return v1; }

        public static GVector2 Divide(GVector2 v1, GVector2 v2) { return new GVector2(v2.x > GMath.EPS ? v1.x : (v1.x / v2.x), v2.y > GMath.EPS ? v1.y : (v1.y / v2.y)); }

        public static GVector2 operator /(GVector2 v1, float v) { return Divide(v1, v); }

        public static GVector2 operator /(GVector2 v1, GVector2 v2) { return Divide(v1, v2); }

        #endregion

        #region Lerp

        public static GVector2 Lerp(GVector2 v1, GVector2 v2, float lerp) { return v1 + (v2 - v1) * lerp;  }

        public static GVector2 SmoothStep(GVector2 v1, GVector2 v2, float lerp) { return Lerp(v1, v2, lerp.Pow2() * (3 - 2 * lerp)); }

        public static GVector2 CatmullRom(GVector2 v1, GVector2 v2, GVector2 v3, GVector2 v4, float lerp) {  
            float amountPow2 = lerp.Pow2();
            float amountPow3 = amountPow2 * lerp; 
            return new GVector2(
                    ((2f * v2.x + (-v1.x + v3.x) * lerp + (2f * v1.x - 5f * v2.x + 4f * v3.x - v4.x) * amountPow2 + (3f * v2.x - 3f * v3.x - v1.x + v4.x) * amountPow3) * 0.5f),
                    ((2f * v2.y + (-v1.y + v3.y) * lerp + (2f * v1.y - 5f * v2.y + 4f * v3.y - v4.y) * amountPow2 + (3f * v2.y - 3f * v3.y - v1.y + v4.y) * amountPow3) * 0.5f)
            );
        }

        public static GVector2 Hermite(GVector2 value1, GVector2 tangent1, GVector2 value2, GVector2 tangent2, float lerp) { 
            float lerpPow2 = lerp.Pow2();
            float lerpPow3 = lerpPow2 * lerp; 
            float h1 = 2 * lerpPow3 - 3 * lerpPow2 + 1;
            float h2 = -2 * lerpPow3 + 3 * lerpPow2;
            float h3 = lerpPow3 - 2 * lerpPow2 + lerp;
            float h4 = lerpPow3 - lerpPow2; 
            return new GVector2(
                    h1 * value1.x + h2 * value2.x + h3 * tangent1.x + h4 * tangent2.x,
                    h1 * value1.y + h2 * value2.y + h3 * tangent1.y + h4 * tangent2.y
            );
        }

        #endregion
         
        public static float Dot(GVector2 v1, GVector2 v2) { return v1.x * v2.x + v1.y * v2.y; }

        public static float Distance(GVector2 v1, GVector2 v2) { return GMath.Sqrt(Distance2(v1, v2)); }

        public static float Distance2(GVector2 v1, GVector2 v2) { return (v1.x - v2.x).Pow2() + (v1.y - v2.y).Pow2(); }

        /// <summary>
        /// 向量夹角
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>返回值范围：[0,360)</returns> 
        public static float Angle(GVector2 v1, GVector2 v2) {
            float v = GMath.Atan(v1.x, v1.y) - GMath.Atan(v2.x, v2.y);
            if (v < 0) v += 360;
            return v;
        }

        

        public override string ToString() { return "x:" + x + " y:" + y; }
    }


    public struct GVector3 {
        public float x;
        public float y;
        public float z;

        public readonly static GVector3 Zero = new GVector3(0, 0, 0);
        public readonly static GVector3 One = new GVector3(1, 1, 1);

        public GVector3(float _x, float _y, float _z) { x = _x; y = _y; z = _z; }
        public GVector3(GVector3 origin) { x = origin.x; y = origin.y; z = origin.z; }

        public float this[int i] {
            get { return i == 0 ? x : i == 1 ? y : i == 2 ? z : -999999; }
            set {
                switch (i) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                }
            }
        }

        public float magnitude { get { return GMath.Sqrt(x * x + y * y + z * z); } }

        public GVector3 normal {
            get {
                float l = magnitude;
                if (l > GMath.EPS) return new GVector3(x / l, y / l, z / l);
                return Zero;
            }
        }

        public static GVector3 operator +(GVector3 v1, GVector3 v2) {
            return new GVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static GVector3 operator -(GVector3 v1, GVector3 v2) {
            return new GVector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static GVector3 operator *(GVector3 v1, float v) {
            return new GVector3(v1.x * v, v1.y * v, v1.z * v);
        }

        public static GVector3 operator *(float v, GVector3 v1) {
            return new GVector3(v1.x * v, v1.y * v, v1.z * v);
        }

        public static GVector3 operator /(GVector3 v1, float v) {
            if (GMath.Abs(v) > GMath.EPS)
                return new GVector3(v1.x / v, v1.y / v, v1.z / v);
            return v1;
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }


        public static bool operator ==(GVector3 v1, GVector3 v2) {
            if ((object)v1 != null || (object)v2 != null) return false;
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(GVector3 v1, GVector3 v2) {
            return !(v1 == v2);
        }

        public static float Distance(GVector3 v1, GVector3 v2) {
            return GMath.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
        }

        public static float Dot(GVector3 v1, GVector3 v2) {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
    }
}

