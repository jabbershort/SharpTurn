using System;

namespace SharpTurn
{
    /// <summary>
    /// The Class Rotation represents a 3D rotation. It can be constructed from either Euler Angles, Quaternion or Rotational Matrix. Internally, the rotations are stored as quaternions.
    /// </summary>
    public class Rotation
    {
        /// <summary>
        /// X: The Rotation class utilises the standard X,Y,Z,W Quaternion notation. Where X,Y,Z represent the imaginary elements and W is the real part of the rotation.
        /// </summary>
        public float x;
        /// <summary>
        /// Y: The Rotation class utilises the standard X,Y,Z,W Quaternion notation. Where X,Y,Z represent the imaginary elements and W is the real part of the rotation.
        /// </summary>
        public float y;
         /// <summary>
        /// Z: The Rotation class utilises the standard X,Y,Z,W Quaternion notation. Where X,Y,Z represent the imaginary elements and W is the real part of the rotation.
        /// </summary>
        public float z;
        /// <summary>
        /// W: The Rotation class utilises the standard X,Y,Z,W Quaternion notation. Where X,Y,Z represent the imaginary elements and W is the real part of the rotation.
        /// </summary>
        public float w;

        /// <summary>
        /// Private property for easy conversion between radians and degrees.
        /// </summary>
        private static float Rad2Deg = 57.2958f;
                /// <summary>
        /// Private property for easy conversion between degrees and radians.
        /// </summary>
        private static float Deg2Rad = 0.0174533f;


        /// <summary>
        /// Standard constructor for an instance of the rotation class.
        /// </summary>
        /// <param name="x">X part of the quaternion.</param>
        /// <param name="y">Y part of the quaternion.</param>
        /// <param name="z">Z part of the quaternion.</param>
        /// <param name="w">W part of the quaternion.</param>
        public Rotation(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        /// <summary>
        /// AsEuler is an instance method to return a set of Euler angles from the Rotation class.
        /// </summary>
        /// <returns>Float array of the Euler angles in degrees.</returns>
        public float[] AsEuler()
        {
            // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/index.htm
            float[] euler = new float[3];

            double sinr_cosp = 2 * (w * x + y * z);
            double cosr_cosp = 1 - 2 * (x * x + y * y);
            euler[0] = Rad2Deg*(float)Math.Atan2(sinr_cosp,cosr_cosp);

            double sinp = 2 * (w * y - z * x);
            if (Math.Abs(sinp) >= 1)
                euler[1] = Rad2Deg * (float)Math.Abs(Math.PI / 2)*Math.Sign(sinp);
            else
                euler[1] = Rad2Deg * (float)Math.Asin(sinp);

            double siny_cosp = 2 * (w * z + x * y);
            double cosy_cosp = 1 - 2 * (y * y + z * z);
            euler[2] = Rad2Deg * (float)Math.Atan2(siny_cosp, cosy_cosp);

            return euler;
        }

        /// <summary>
        /// AsQuat is an instance method to return the quaternion from the Rotation class.
        /// </summary>
        /// <returns>float array of the quaternion, in the order x,y,z,w.</returns>
        public float[] AsQuat()
        {
            return new float[4]{x,y,z,w};            
        }

        /// <summary>
        /// AsMatrix is an instance method to return a rotational matrix from the Rotation class.
        /// </summary>
        /// <returns>Float array of the rotational matrix, in rows.</returns>
        public float[,] AsMatrix()
        {
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
            float[,] matrix = new float[3,3]{
                {
                    (float)(1 - 2*Math.Pow(y,2) - 2*Math.Pow(z,2)),
                    2*x*y - 2*z*w,
                    2*x*z + 2*y*w
                },
                {
                    2*x*y + 2*z*w,
 	                (float)(1 - 2*Math.Pow(x,2) - 2*Math.Pow(z,2)),
     	            2*y*z - 2*x*w
                },
                {
                    2*x*z - 2*y*w,
 	                2*y*z + 2*x*w,
                    (float)(1 - 2*Math.Pow((double)x,2) - 2*Math.Pow((double)y,2)) 
                }
            };
            return matrix;
        }

        /// <summary>
        /// FromEuler is a static constructor to create a Rotation from euler angles
        /// </summary>
        /// <param name="angles">Float array of the euler angles in degrees.</param>
        /// <returns>Rotation</returns>
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

        /// <summary>
        /// FromQuat is a static constructor to create a rotation from quaternion.
        /// </summary>
        /// <param name="quat">Float array of the quaternion in order x,y,z,w.</param>
        /// <returns>Rotation</returns>
        public static Rotation FromQuat(float[] quat)
        {
            return new Rotation(quat[0],quat[1],quat[2],quat[3]);
        }

        /// <summary>
        /// From Matrix is a static constructor to create a rotation from rotational matrix.
        /// </summary>
        /// <param name="matrix">Float array of the rotaitonal matrix in rows.</param>
        /// <returns>Rotation</returns>
        public static Rotation FromMatrix(float[,] matrix)
        {
            // https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/

            // version 1
            // float x,y,z,w;
            // float trace = matrix[0,0] + matrix[1,1] + matrix[2,2]; // I removed + 1.0f; see discussion with Ethan
            // if( trace > 0 ) 
            // {// I changed M_EPSILON to 0
            //     float s = 0.5f / (float)Math.Sqrt(trace+ 1.0f);
            //     w = 0.25f / s;
            //     x = ( matrix[2,1] - matrix[1,2] ) * s;
            //     y = ( matrix[0,2] - matrix[2,0] ) * s;
            //     z = ( matrix[1,0] - matrix[0,1] ) * s;
            // } 
            // else 
            // {
            //     if ( matrix[0,0] > matrix[1,1] && matrix[0,0] > matrix[2,2] ) 
            //     {
            //         float s = 2.0f * (float)Math.Sqrt( 1.0f + matrix[0,0] - matrix[1,1] - matrix[2,2]);
            //         w = (matrix[2,1] - matrix[1,2] ) / s;
            //         x = 0.25f * s;
            //         y = (matrix[0,1] + matrix[1,0] ) / s;
            //         z = (matrix[0,2] + matrix[2,0] ) / s;
            //     } 
            //     else if (matrix[1,1] > matrix[2,2]) 
            //     {
            //         float s = 2.0f * (float)Math.Sqrt( 1.0f + matrix[1,1] - matrix[0,0] - matrix[2,2]);
            //         w = (matrix[0,2] - matrix[2,0] ) / s;
            //         x = (matrix[0,1] + matrix[1,0] ) / s;
            //         y = 0.25f * s;
            //         z = (matrix[1,2] + matrix[2,1] ) / s;
            //     } 
            //     else 
            //     {
            //         float s = 2.0f * (float)Math.Sqrt( 1.0f + matrix[2,2] - matrix[0,0] - matrix[1,1] );
            //         w = (matrix[1,0] - matrix[0,1] ) / s;
            //         x = (matrix[0,2] + matrix[2,0] ) / s;
            //         y = (matrix[1,2] + matrix[2,1] ) / s;
            //         z = 0.25f * s;
            //     }
            // }
            // Version 2
            float w= (float)Math.Sqrt(1 + (double)matrix[0,0] + (double)matrix[1,1] + (double)matrix[2,2]) /2;
            float x = (matrix[2,1] - matrix[1,2])/(4 *w);
            float y = (matrix[0,2] - matrix[2,0])/( 4 *w);
            float z = (matrix[1,0] - matrix[0,1])/( 4 *w);
            Rotation r = new Rotation(x,y,z,w);
            // WTF, why is this the way it works best !!
            // TODO: Fix this shitty method.
            float[] euler = r.AsEuler();
            return Rotation.FromEuler(euler);
        }
    }
}