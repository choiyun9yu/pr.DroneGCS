// using kisa_gcs_system.Services;
//
// namespace kisa_gcs_system.Controllers;
//
// // [ApiController]
// // [EnableCors("CorsPolicy")]
// // [Route("/addnewdrone")]
// public class AddNewDrone : ControllerBase
// {
//
//     public static async Task DroneUdpConnection(IHost host)
//     {
//         var mavlinkNettyService =
//             (MavlinkNetty)host 
//                 .Services.GetService(typeof(MavlinkNetty))!;                
//         await mavlinkNettyService.Bind(_port); 
//     }
//     
//     // private static string _protocol = "UDP";
//     // private static string _address = "127.0.0.1";
//     private static int _port = 14556;
//
//     
//     // [HttpPost("upd")]
//     // public IActionResult AddNewConnection()
//     // {
//     //     Console.WriteLine("received");
//     //     try
//     //     {
//     //         var form = Request.Form;
//     //         _protocol = form["protocol"];
//     //         _address = form["address"];
//     //         _port = int.Parse(form["port"]);
//     //         Console.WriteLine(_protocol);
//     //         Console.WriteLine(_address);
//     //         Console.WriteLine(_port);
//     //         return Ok("Add New Drone Successfully.");
//     //     }
//     //     catch (Exception e)
//     //     {
//     //         Console.WriteLine(e);
//     //         return BadRequest("Error: Can't Add New Drone.");
//     //     }
//     // }
//     
// }