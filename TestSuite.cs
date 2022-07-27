using System;

namespace SharpTurn
{
    class RotationTester
    {
        public static float quatTolerance = 0.1f;
        public static float eulerTolerance = 1f;
        public static float matrixTolerance = 0.1f;
        
        public static bool EulerToQuat(float[] euler, float[] expectedQuat, bool verbose = false)
        {
            Rotation r = Rotation.FromEuler(euler);
            float[] calculatedQuat = r.AsQuat();

            float totalError = 0;  
            for (int i = 0;i<calculatedQuat.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<quatTolerance) 
            { 
                if (verbose) { Console.WriteLine(string.Format("Euler To Quaternion test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedQuat),DebugUtility.FloatArrayAsString(calculatedQuat),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Euler To Quaternion test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedQuat),DebugUtility.FloatArrayAsString(calculatedQuat),totalError));}
                return false;
            }
        }
        public static bool EulerToMatrix(float[] euler, float[,] expectedMatrix, bool verbose = false)
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
            if (totalError< matrixTolerance)            
            { 
                if (verbose) { Console.WriteLine(string.Format("Euler To Matrix test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedMatrix),DebugUtility.FloatArrayAsString(calculatedMatrix),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Euler To Matrix test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedMatrix),DebugUtility.FloatArrayAsString(calculatedMatrix),totalError));}
                return false;
            }

        }
        public static bool QuatToEuler(float[] quat, float[] expectedEuler, bool verbose = false)
        {
            Rotation r = Rotation.FromQuat(quat);
            float[] calculatedEuler = r.AsEuler();

            float totalError = 0;  
            for (int i = 0;i<calculatedEuler.Length;i++)
            {
                totalError += Math.Abs(expectedEuler[i]-calculatedEuler[i]);
            }
            if (totalError<eulerTolerance)
            { 
                if (verbose) { Console.WriteLine(string.Format("Quaternion To Euler test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedEuler),DebugUtility.FloatArrayAsString(calculatedEuler),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Quaternion To Euler test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedEuler),DebugUtility.FloatArrayAsString(calculatedEuler),totalError));}
                return false;
            }
        }
        public static bool QuatToMatrix(float[] quat, float[,] expectedMatrix, bool verbose = false)
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
            if (totalError< matrixTolerance)
            { 
                if (verbose) { Console.WriteLine(string.Format("Quaternion To Matrix test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedMatrix),DebugUtility.FloatArrayAsString(expectedMatrix),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Quaternion To Matrix test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedMatrix),DebugUtility.FloatArrayAsString(expectedMatrix),totalError));}
                return false;
            }
        }
        public static bool MatrixToEuler(float[,] matrix, float[] expectedEuler, bool verbose = false)
        {
            Rotation r = Rotation.FromMatrix(matrix);
            float[] calculatedEuler = r.AsEuler();

            float totalError = 0;  
            for (int i = 0;i<calculatedEuler.Length;i++)
            {
                totalError += Math.Abs(expectedEuler[i]-calculatedEuler[i]);
            }
            if (totalError<eulerTolerance) 
            { 
                if (verbose) { Console.WriteLine(string.Format("Matrix To Euler test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedEuler),DebugUtility.FloatArrayAsString(calculatedEuler),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Matrix To Euler test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedEuler),DebugUtility.FloatArrayAsString(calculatedEuler),totalError));}
                return false;
            }
        }
        public static bool MatrixToQuat(float[,] matrix, float[] expectedQuat, bool verbose = false)
        {
            // TODO: getting 10% failure rate.
            Rotation r = Rotation.FromMatrix(matrix);
            float[] calculatedQuat = r.AsQuat();
            
            float totalError = 0;  
            for (int i = 0;i<calculatedQuat.Length;i++)
            {
                totalError += Math.Abs(expectedQuat[i]-calculatedQuat[i]);
            }
            if (totalError<quatTolerance)
            { 
                if (verbose) { Console.WriteLine(string.Format("Matrix To Quaternion test passed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedQuat),DebugUtility.FloatArrayAsString(calculatedQuat),totalError));}
                return true;
            }
            else 
            { 
                if (verbose) { Console.WriteLine(string.Format("Matrix To Quaternion test failed. Expected Value: {0}. Calculated Value: {1}. Error: {2}.",DebugUtility.FloatArrayAsString(expectedQuat),DebugUtility.FloatArrayAsString(calculatedQuat),totalError));}
                return false;
            }
        }

    }
    public static class DebugUtility
    {

        public static string FloatArrayAsString(float[] arr,int dp = 2)
        {
            string output = "";
            for (int i = 0;i<arr.Length;i++)
            {
                output += Math.Round(arr[i],dp).ToString();
                output += ", ";
            }
            return output;
        }
        public static string FloatArrayAsString(float[,] arr, int dp = 2)
        {
            string output = string.Format("[{0}],[{1}],[{2}]",
                FloatArrayAsString(new float[3]{arr[0,0],arr[0,1],arr[0,2]}),
                FloatArrayAsString(new float[3]{arr[1,0],arr[1,1],arr[1,2]}),
                FloatArrayAsString(new float[3]{arr[2,0],arr[2,1],arr[2,2]})
            );
            return output;
        }
    }
}