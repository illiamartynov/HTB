using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTB;

using System;
using System.ComponentModel.DataAnnotations;

public class Certificate
{
    private static List<Certificate> _extent = new List<Certificate>();
    public static IReadOnlyList<Certificate> Extent => _extent.AsReadOnly();
    
    private int _certificateId;
    private DateTime _issueDate;

    [Required(ErrorMessage = "Certificate ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Certificate ID must be a positive integer.")]
    public int CertificateId
    {
        get => _certificateId;
        private set => _certificateId = value;
    }

    [Required(ErrorMessage = "Issue Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Issue Date must be a valid date.")]
    public DateTime IssueDate
    {
        get => _issueDate;
        private set => _issueDate = value;
    }

    public Person Owner { get; private set; }
    
    public Certificate(int certificateId, DateTime issueDate, Person owner)
    {
        CertificateId = certificateId;
        IssueDate = issueDate;
        if (owner != null)
            AddOwner(owner);
        
        _extent.Add(this);
    }

    public static void RemoveCertificate(Certificate certificate)
    {
        certificate.RemoveOwner(certificate.Owner);
        _extent.Remove(certificate);
    }
    
    // person funcs
    public void AddOwner(Person person)
    {
        if (person == null)
            throw new ArgumentNullException(nameof(person));

        if (!person.Equals(Owner))
        {
            person.AddCertificateReverse(this);
            Owner = person;
        }
    }

    public void RemoveOwner(Person person)
    {
        if (person != null)
            throw new ArgumentNullException(nameof(person));

        person.RemoveCertificateReverse(this);
        Owner = null;
    }

    public void UpdateOwner(Person oldPerson, Person newPerson)
    {
        if (newPerson == null) 
            throw new ArgumentNullException(nameof(newPerson));
        if (oldPerson == null)
            throw new ArgumentNullException(nameof(oldPerson));
        
        if (Owner != oldPerson)
            throw new InvalidOperationException("PersonToRemove isn't current Owner of certificate");
        
        // disassociate oldPerson
        Owner = null;
        oldPerson.RemoveCertificateReverse(this);
        
        // associate newPerson
        AddOwner(newPerson);
    }
    
    // reverse person funcs
    public void AddOwnerReverse(Person owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public void RemoveOwnerReverse()
    {
        Owner = null;
    }
    

    // other funcs
    public void ViewCertificate()
    {
        Console.WriteLine($"Certificate ID: {CertificateId}, Issue Date: {IssueDate}, Owner: {Owner.Name}");
    }
    
    // extent funcs
    public static void LoadExtent(string filename = "certificate_extent.json")
    {
        if (File.Exists(filename))
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            try
            {
                var json = File.ReadAllText(filename);
                _extent = JsonSerializer.Deserialize<List<Certificate>>(json, options) ?? new List<Certificate>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the extent: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"File '{filename}' not found. Initializing an empty extent.");
            _extent = new List<Certificate>();
        }
    }
    
    public static void SaveExtent(string filename = "certificate_extent.json")
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        try
        {
            var json = JsonSerializer.Serialize(_extent, options);
            File.WriteAllText(filename, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the extent: {ex.Message}");
        }
    }
}
