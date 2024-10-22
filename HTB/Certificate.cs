namespace HTB;

using System;


public class Certificate
{
    public int CertificateId { get; set; }
    public DateTime IssueDate { get; set; }

    
    public Certificate(int certificateId, DateTime issueDate)
    {
        CertificateId = certificateId;
        IssueDate = issueDate;
    }

    
    public void GenerateCertificate()
    {
        Console.WriteLine($"Certificate {CertificateId} generated.");
    }

    
    public void ViewCertificate()
    {
        Console.WriteLine($"Certificate ID: {CertificateId}, Issue Date: {IssueDate}");
    }
}
