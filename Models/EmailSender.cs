using DGSappSem2.Models.Students;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;


namespace DGSappSem2.Models
{
    public class EmailSender
    {
        private readonly static ApplicationDbContext db = new ApplicationDbContext();
        public static void SendRegistrationEmail(Student studentApplication, byte[] pdf)
        {
            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(pdf), "Proof of registration", "application/pdf"));
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.StudentEmail, studentApplication.StudentName));
            var body = $"Good Day {studentApplication.StudentName}, \n\n {studentApplication.StudentName} has been successfully registered at our school. Please Find the attached registration information(Proof of Registration)<br/> Regards,<br/><br/> New Erra <br/> .";
            DGSappSem2.Models.Emai_Service emailService = new DGSappSem2.Models.Emai_Service();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + studentApplication.StID,
                mailBody = body,
                mailFooter = "<br/> <br/> <b>New Era</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments
            });
        }
        public static void SendRegistrationConfEmail(Student studentApplication)
        {
            var attachments = new List<Attachment>();
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.StudentEmail, studentApplication.StudentName));
            var body = $"Good Day {studentApplication.StudentName}, \n\n {studentApplication.StudentName} has been successfully registered at our school. Please Find the attached registration information(Proof of Registration)<br/> Regards,<br/><br/> New Erra <br/> .";
            DGSappSem2.Models.Emai_Service emailService = new DGSappSem2.Models.Emai_Service();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + studentApplication.StID,
                mailBody = body,
                mailFooter = "<br/> <br/> <b>New Era</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments
            });
        }

    }
}