using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Shared.Constants
{
    public class SemesterConstants
    {
        public static string INVALID_REG_START_DATE = "Invalid registration start date";
        public static string INVALID_REG_END_DATE = "Invalid registration end date";
        public static string INVALID_SEM_START_DATE = "Invalid semester start date";
        public static string INVALID_SEM_END_DATE = "Invalid semester end date";
        public static string INVALID_CREDIT_LIMIT = "Invalid credit limit";
        public static string REG_START_DATE_GT_END_DATE = "Registration start date is greater than end date";
        public static string SEM_START_DATE_GT_END_DATE = "Semester start date is greater than end date";
        public static string INVALID_REGISTRATION_STATUS = "Invalid registration status";
        public static string NO_CURRENT_SEM = "No current semester";
    }
}