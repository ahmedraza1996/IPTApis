using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace IptApis.Models.JobPortal
{
    public class Job
    {
        public int JobID { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string LastApplyDate { get; set; }
        public string Designation { get; set; }
        public int MinExperience { get; set; }
        public string AttachmentPath { get; set; }
        public string ApplicationLink { get; set; }
        public string Contactperson { get; set; }
        public Job setAll(Job newJob)
        {
            Title = newJob.Title;
            Organization = newJob.Organization;
            LastApplyDate = newJob.LastApplyDate;
            Designation = newJob.Designation;
            MinExperience = newJob.MinExperience;
            AttachmentPath = newJob.AttachmentPath;
            ApplicationLink = newJob.ApplicationLink;
            Contactperson = newJob.Contactperson;
            return this;
        }

        public static implicit operator ReadOnlyDictionary<object, object>(Job v)
        {
            throw new NotImplementedException();
        }
    }
}