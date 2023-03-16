using System.Collections.Generic;
using UnityEngine;

//static functions 
//basic maths stuff & generic list stuff 

namespace Util
{
    public class ListUtils
    {
        //return same list removing dupes
        public static List<T> GetDistinctElems<T>(List<T> list)
        {
            List<T> distinctElems = new List<T>();
            foreach (T elem in list)
            {
                if (!distinctElems.Contains(elem))
                {
                    distinctElems.Add(elem);
                }
            }
            return distinctElems;
        }

        //returns elems in list 1 but not list 2
        public static List<T> GetMissingElements<T>(List<T> requiredList, List<T> myList)
        {
            List<T> missingElems = new List<T>();

            foreach (T elem in requiredList)
            {
                if (!myList.Contains(elem))
                {
                    missingElems.Add(elem);
                }
            }

            return missingElems;
        }

        //returns elements that are in both lists
        public static List<T> GetSharedListElements<T>(List<T> referenceList, List<T> myList)
        {
            List<T> shareElems = new List<T>();
            foreach (T elem in referenceList)
            {
                if (myList.Contains(elem))
                {
                    shareElems.Add(elem);
                }
            }
            return shareElems;
        }

    }

    public class MathUtils
    {
        
        //returns the int distance of a number from designated range
        public static int GetDistanceFromRange(int min,int max,int myNum)
        {
            if (myNum > max)
            {
                return max - myNum;
            }
            else if (myNum < min)
            {
                return min - myNum;
            }
            return 0;
        }
        public float StandardDeviation(float[] x)
        {
            float pv = 0; //population variance
            float mu = MeanValue(x);
            for (int i = 0; i < x.Length; i++)
            {
                pv += (x[i] - mu) * (x[i] - mu);
            }
            pv /= x.Length;
            return Mathf.Sqrt(pv);
        }
        public float MeanValue(float[] x) //average
        {
            return Sum(x) / (x.Length);
        }
        public float Sum(float[] x)
        {
            float s = 0;
            for (int i = 0; i < x.Length; i++)
            {
                s += x[i];
            }
            return s;
        }


    }




}