using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SharpTurn
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
                float[] euler = entry["euler"].ToObject<float[]>();
                float[] quat = entry["quat"].ToObject<float[]>();
                float[,] matrix = JArrayToMatrix(entry["matrix"]);
                bool e2q = RotationTester.EulerToQuat(euler,quat,true);
                if (e2q){e2q_total++;}
                bool e2m = RotationTester.EulerToMatrix(euler,matrix,true);
                if (e2m){e2m_total++;}
                bool q2e = RotationTester.QuatToEuler(quat,euler,true);
                if (q2e){q2e_total++;}
                bool q2m = RotationTester.QuatToMatrix(quat,matrix,true);
                if (q2m){q2m_total++;}
                bool m2e = RotationTester.MatrixToEuler(matrix,euler,true);
                if (m2e){m2e_total++;}
                bool m2q = RotationTester.MatrixToQuat(matrix,quat,true);
                if (m2q){m2q_total++;}
            }
            string results = string.Format("Run {0} tests, with the following pass rates. \n Euler To Quaternion: {1}. \n Euler To Matrix: {2}. \n Quaternion To Euler: {3}. \n Quaternion To Matrix: {4}. \n Matrix To Euler: {5}. \n Matrix To Quaternion: {6}. \n",num_tests,e2q_total,e2m_total,q2e_total,q2m_total,m2e_total,m2q_total);
            Console.WriteLine(results);
        }

        private static float[,] JArrayToMatrix(Newtonsoft.Json.Linq.JArray jarr)
        {
            float[,] matrix = new float[3,3];
            for(int i = 0;i<jarr.Count;i++)
            {
                float[] minorArray = jarr[i].ToObject<float[]>();
                for(int j = 0;j<minorArray.Length;j++)
                {
                    matrix[i,j]= minorArray[j];
                }
            }
            return matrix;
        }

    }
}
