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
            int e2q_total = 0;
            int e2m_total = 0;
            int q2e_total = 0;
            int q2m_total = 0;
            int m2e_total = 0;
            int m2q_total = 0;
            int num_tests = data.Count;
            foreach(Dictionary<string,dynamic> entry in data)
            {
                float[] euler = entry["euler"];
                float[] quat = entry["quat"];
                float[][] matrix = entry["matrix"];
                Console.WriteLine(entry["euler"][0]);
                bool e2q = RotationTester.EulerToQuat(euler,quat);
                bool e2m = RotationTester.EulerToMatrix(euler,matrix);
                bool q2e = RotationTester.QuatToEuler(quat,euler);
                bool q2m = RotationTester.QuatToMatrix(quat,matrix);
                bool m2e = RotationTester.MatrixToEuler(matrix,euler);
                bool m2q = RotationTester.MatrixToQuat(matrix,quat);

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
            float[] calculatedQuat = r.AsQuat();

            float totalError = 0;  
            for (int i = 0;i<qCalc.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<quatTolerance) { return true;}
            else { return false;}
        }
        public static bool EulerToMatrix(float[] euler, float[,] expectedMat)
        {
            Rotation r = Rotation.FromEuler(euler);
            float[,] calculatedMatrix = r.AsMatrix();

            float totalError = 0;
            for (int i = 0;i<3;i++)
            {
                for (int j =0;j<3;j++)
                {
                    totalError += Math.Abs(expectedMatrix[i,j]-calculatedMatrix[i,j]);
                }
            }
            if (totalError< matrixToleranc) { return true;}
            else { return false;}

        }
        public static bool QuatToEuler(float[] quat, float[] expectedEuler)
        {
            Rotation r = Rotation.FromQuat(quat);
            float[] calculatedEuler = r.AsEuler();

            float totalError = 0;  
            for (int i = 0;i<qCalc.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<eulerTolerance) { return true;}
            else { return false;}
        }
        public static bool QuatToMatrix(float[] quat, float[,] expectedMat)
        {
            Rotation r = Rotation.FromQuat(quat);
            float[,] calculatedMatrix = r.AsMatrix();
            
            float totalError = 0;
            for (int i = 0;i<3;i++)
            {
                for (int j =0;j<3;j++)
                {
                    totalError += Math.Abs(expectedMatrix[i,j]-calculatedMatrix[i,j]);
                }
            }
            if (totalError< matrixToleranc) { return true;}
            else { return false;}
        }
        public static bool MatrixToEuler(float[,] matrix, float[] expectedEuler)
        {
            Rotation r = Rotation.FromMatrix(matrix);
            float[] calculatedEuler = r.AsEuler();

            float totalError = 0;  
            for (int i = 0;i<qCalc.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<eulerTolerance) { return true;}
            else { return false;}
        }
        public static bool MatrixToQuat(float[,] matrix, float[] expectedQuat)
        {
            Rotation r = Rotation.FromMatrix(matrix);
            float[] calculatedQuat = r.AsQuat();
            
            float totalError = 0;  
            for (int i = 0;i<qCalc.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<quatTolerance) { return true;}
            else { return false;}
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
            float[] euler = new float[3];

            double sinr_cosp = 2 * (w * x + y * z);
            double cosr_cosp = 1 - 2 * (x * x + y * y);
            euler[0] = Rad2Deg*Math.Atan2(sinr_cosp,cosr_cosp);

            double sinp = 2 * (w * y - z * x);
            if (Math.Abs(sinp) >= 1)
                euler[1] = Rad2Deg * Math.Abs(Math.PI / 2)*Math.Sign(sinp);
            else
                euler[1] = Rad2Deg * Math.Asin(sinp);

            double siny_cosp = 2 * (w * z + x * y);
            double cosy_cosp = 1 - 2 * (y * y + z * z);
            euler[2] = Rad2Deg * Math.Atan2(siny_cosp, cosy_cosp);

            return euler;
        }

        public float[] AsQuat()
        {
            return new float[4]{x,y,z,w};            
        }
        public float[,] AsMatrix()
        {
            // TODO: convert quat to matrix
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
            float[] row1 = new float[3]{
                1 - 2*Math.Pow(y,2) - 2*Math.Pow(z,2),
                2*x*y - 2*z*w,
                2*x*z + 2*y*w
            };
            float[] row2 = new float[3]{
                2*x*y + 2*z*w,
 	            1 - 2*Math.Pow(x,2) - 2*Math.Pow(z,2),
     	        2*y*z - 2*x*w
            };
            float[] row3 = new float[3]{
                2*x*z - 2*y*w,
 	            2*y*z + 2*x*w,
                1 - 2*Math.Pow(x,2) - 2*Math.Pow(y,2) 
            };
            return new float[,] = new float[3,3]{row1,row2,row3};
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
            float w= Math.Sqrt(1 + row1[0] + row2[1] + row3[2]) /2;
            float x = (row3[1] - row2[2])/(4 *w);
            float y = (row1[2] - row3[0])/( 4 *w);
            float z = (row2[0] - row1[1])/( 4 *w);
            return new Rotation(x,y,z,w);
        }


    }
}
