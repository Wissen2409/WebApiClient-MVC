using System.Diagnostics;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApiClient_MVC.Models;

namespace WebApiClient_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

        // yazmış olduğumuz rest apiye headerD'dan veri göndererek token verisini alalım!!

        var client = new RestClient("http://wissenwebapi.runasp.net");
        var request = new RestRequest("/api/Auth/login", Method.Post);

        // Body içerisinde json gönderiyoruz!!
        var body = new { Username = "root", Password = "1010" };
        request.AddJsonBody(body);
        var response = client.Execute(request);
        string jwtToken = response.Content;

        return RedirectToAction("Privacy","Home",new {token=jwtToken});




        return View();
    }

    public IActionResult Privacy(string token)
    {
        // üstteki metotdan token alındı!!, tokenı kullanarak web apinin token tarafından korunan bir metoduna istek yapalım!!

        var client = new RestClient("http://wissenwebapi.runasp.net");
        var request = new RestRequest("/api/Header?model='Fatma", Method.Post);

        // artık bu istekle, authentication ile korunan bir apiye istek atacağız, 
        // token bilgisi, header ile gönderilmelidir!!
        // restcharp ile header eklemek aşağıdaki gibidir!!
        string tokenString = token.Replace("/","").Replace('"',' ').Trim();
        request.AddHeader("Authorization","Bearer "+tokenString);
        request.AddHeader("Content-Type","application/json");

        // Body içerisinde json gönderiyoruz!!
        //var body = new { Id = 1, Name = "fatma" };
        //request.AddJsonBody(body);
        var response = client.Execute(request);
         
        string content = response.Content;


        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
