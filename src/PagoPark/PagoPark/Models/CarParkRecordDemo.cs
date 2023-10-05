namespace PagoPark.Models;

public class CarParkRecordDemo
{
    public string Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public double Pay { get; set; }
    public string VehicleRegistration { get; set; }  
    public string Observations { get; set; }

    public CarParkRecordDemo(){ }
    public CarParkRecordDemo(DateTime registrationdate, double pay, string vehicleregistration, string observations)
    {
        RegistrationDate = registrationdate;
        Pay = pay;
        VehicleRegistration = vehicleregistration;
        Observations = observations;
    }
}
