using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWeb2017.framework
{
    public static class ExtendHelper
    {
        public static int ToInt(this string s)
        {
            int i;
            int.TryParse(s, out  i);
            return i;
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime dt;
            DateTime.TryParse(value, out dt);
            return dt;
        }

        public static int BinarySearchIndex(this int[] nums, int key) //用折半查找，查找用户输入的数字  
        {
            int low = 0;
            int high = nums.Length - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;
                if (key > nums[middle])
                {
                    low = middle + 1;   //查找数组后部分  
                }
                else if (key < nums[middle])
                {
                    high = middle - 1;  //查找数组前半部分  
                }
                else
                {
                    return middle;  //找到用户要查找的数字，返回下标  
                }
            }

            return -1;  //没有找到用户查找的数字，返回-1  
        }
    }
}