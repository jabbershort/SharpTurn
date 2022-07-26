using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace rotation_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Dictionary<string,dynamic>> data = new List<Dictionary<string, dynamic>>();
            using (StreamReader file = File.OpenText("rots.json"))
            {
                string jsonString = file.ReadToEnd();
                data = JsonConvert.DeserializeObject<List<Dictionary<string,dynamic>>>(jsonString);
            }
            foreach(Dictionary<string,dynamic> entry in data)
            {
                Console.WriteLine(entry["euler"][0]);
            }
        }
    }

    class RotationTester
    {
        public static float quatTolerance = 0.1f;
        public static float eulerTolerance = 1f;
        public static float matrixTolerance = 0.1f;
        public static bool EulerToQuat(float[] euler, float[] expectedQuat)
        {
            Rotation r = Rotation.FromEuler(euler);
            float[] qCalc = r.AsQuat();

            float totalError = 0;  
            for (int i = 0;i<qCalc.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-qCalc[i]);
            }
            if (totalError<quatTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool EulerToMatrix(float[] euler, float[][] expectedMat)
        {
            bool pass = false;

            // TODO: Test function here.

            return pass;
        }
        public static bool QuatToEuler(float[] quat, float[] expectedEuler)
        {
            bool pass = false;
            
            // TODO: Test function here.

            return pass;
        }
        public static bool QuatToMatrix(float[] quat, float[][] expectedMat)
        {
            bool pass = false;

            // TODO: Test function here.

            return pass;
        }
        public static bool MatrixToEuler(float[][] matrix, float[] expectedEuler)
        {
            bool pass = false;

            // TODO: Test function here.

            return pass;
        }
        public static bool MatrixToQuat(float[][] matrix, float[] expectedQuat)
        {
            bool pass = false;

            // TODO: Test function here.

            return pass;
        }

    }

    class Rotation
    {
        public float x;
        public float y;
        public float z;
        public float w;

        private static float Rad2Deg = 57.2958f;
        private static float Deg2Rad = 0.0174533f;

        public Rotation(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float[] AsEuler()
        {
            // TODO: Convert quat to euler
            // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
            // EulerAngles ToEulerAngles(Quaternion q) {
            //     EulerAngles angles;

            //     // roll (x-axis rotation)
            //     double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            //     double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            //     angles.roll = std::atan2(sinr_cosp, cosr_cosp);

            //     // pitch (y-axis rotation)
            //     double sinp = 2 * (q.w * q.y - q.z * q.x);
            //     if (std::abs(sinp) >= 1)
            //         angles.pitch = std::copysign(M_PI / 2, sinp); // use 90 degrees if out of range
            //     else
            //         angles.pitch = std::asin(sinp);

            //     // yaw (z-axis rotation)
            //     double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            //     double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            //     angles.yaw = std::atan2(siny_cosp, cosy_cosp);

            //     return angles;
            // }
        }

        public float[] AsQuat()
        {
            return new float[4]{x,y,z,w};            
        }
        public float[][] AsMatrix()
        {
            // TODO: convert quat to matrix
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
        }

        public static Rotation FromEuler(float[] angles)
        {  
            // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToQuaternion/index.htm
            float roll = Deg2Rad*angles[0];
            float pitch = Deg2Rad*angles[1];
            float yaw = Deg2Rad*angles[2];

            double cy = Math.Cos(yaw * 0.5);
            double sy = Math.Sin(yaw * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double cr = Math.Cos(roll * 0.5);
            double sr = Math.Sin(roll * 0.5);

            double w = cr * cp * cy + sr * sp * sy;
            double x = sr * cp * cy - cr * sp * sy;
            double y = cr * sp * cy + sr * cp * sy;
            double z = cr * cp * sy - sr * sp * cy;

            return new Rotation((float)x,(float)y,(float)z,(float)w);
        }

        public static Rotation FromQuat(float[] quat)
        {
            return new Rotation(quat[0],quat[1],quat[2],quat[3]);
        }
        public static Rotation FromMatrix(float[] row1, float[] row2, float[] row3)
        {
            // TODO: convert matrix to quat
            https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
        }


    }
}
