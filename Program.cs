using Itinero;
using Itinero.IO.Osm;
using Itinero.Osm.Vehicles;

// Run this the first time to generate .routerdb file
//GenerateRouterDB();

var startPoint = new Itinero.LocalGeo.Coordinate(57.04748f, 9.94281f);
var endPoint = new Itinero.LocalGeo.Coordinate(57.01229f, 9.99261f);
var travelMethod = Vehicle.Pedestrian;

GenerateRoute(startPoint, endPoint, travelMethod);



void GenerateRouterDB()
{
    var routerDb = new RouterDb();
    Itinero.Profiles.Vehicle[] vehicles = { Vehicle.Car, Vehicle.Pedestrian, Vehicle.Bicycle };

    Console.WriteLine("Loading .osm data");
    using (var stream = new FileInfo(@"C:\Users\danpa\Programmering\P6\P6\denmark.osm.pbf").OpenRead())
    {
        routerDb.LoadOsmData(stream, vehicles);
    }

    Console.WriteLine("Writing .routerdb file");
    using (var stream = new FileInfo(@"C:\Users\danpa\Programmering\P6\P6\denmark.routerdb").Open(FileMode.Create))
    {
        routerDb.Serialize(stream);
    }
}


void GenerateRoute(Itinero.LocalGeo.Coordinate startPoint, Itinero.LocalGeo.Coordinate endPoint, Itinero.Profiles.Vehicle travelMethod)
{
    Console.WriteLine("Generating route ...");


    // Opening .routerdb
    RouterDb routerDb;
    using (var stream = new FileInfo(@"..\..\..\denmark.routerdb").OpenRead())
    {
        routerDb = RouterDb.Deserialize(stream);
    }


    // calculating a route.
    var router = new Router(routerDb);

    var profile = travelMethod.Shortest();

    var route = router.Calculate(profile, startPoint, endPoint);

    // Converting to geoJson and writing to file
    var geoJson = route.ToGeoJson();
    File.WriteAllText(@"..\..\..\route.json", geoJson);

    Console.WriteLine("Route generated");
}
