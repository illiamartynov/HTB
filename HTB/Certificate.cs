namespace HTB;

using System;

public class Certificate
{
    private int _certificateId;
    private DateTime _issueDate;

    public int CertificateId
    {
        get => _certificateId;
        private set => _certificateId = value;
    }

    public DateTime IssueDate
    {
        get => _issueDate;
        private set => _issueDate = value;
    }

    public Certificate(int certificateId, DateTime issueDate)
    {
        _certificateId = certificateId;
        _issueDate = issueDate;
    }

    public void GenerateCertificate()
    {
        Console.WriteLine($"Certificate {_certificateId} generated.");
    }

    public void ViewCertificate()
    {
        Console.WriteLine($"Certificate ID: {_certificateId}, Issue Date: {_issueDate}");
    }
}