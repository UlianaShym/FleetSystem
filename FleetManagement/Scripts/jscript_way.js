// When the window has finished loading create our google map below
google.maps.event.addDomListener(window, 'load', init);
var icons = new Array();
var iconImage;
var iconShadow;
var iconShape;
var infowindow;
function getMarkerImage(Text, color) {
    if (color == "red") 
        return  new google.maps.MarkerImage("http://www.google.com/mapfiles/marker" + Text + ".png",
            new google.maps.Size(20, 34),
            new google.maps.Point(0, 0),
            new google.maps.Point(9, 34));    
    else
        return new google.maps.MarkerImage("https://mts.googleapis.com/maps/vt/icon/name=icons/spotlight/spotlight-waypoint-a.png&text="
            + Text
            + "&psize=16&font=fonts/Roboto-Regular.ttf&color=ff333333&ax=44&ay=48&scale=1",
            new google.maps.Size(21, 34),
            new google.maps.Point(0, 0),
            new google.maps.Point(9, 34));
}
        
function init() {
    infowindow = new google.maps.InfoWindow(
        {
            size: new google.maps.Size(150, 50)
        });
    iconShape = {
        coord: [9, 0, 6, 1, 4, 2, 2, 4, 0, 8, 0, 12, 1, 14, 2, 16, 5, 19, 7, 23, 8, 26, 9, 30, 9, 34, 11, 34, 11, 30, 12, 26, 13, 24, 14, 21, 16, 18, 18, 16, 20, 12, 20, 8, 18, 4, 16, 2, 15, 1, 13, 0],
        type: 'poly'
    };
    iconShadow = new google.maps.MarkerImage('http://www.google.com/mapfiles/shadow50.png', new google.maps.Size(37, 34), new google.maps.Point(0, 0), new google.maps.Point(9, 34));
    iconImage = new google.maps.MarkerImage('mapIcons/marker_red.png', new google.maps.Size(20, 34), new google.maps.Point(0, 0), new google.maps.Point(9, 34));
    icons["red"] = iconImage;
    var mapOptions = {
        // How zoomed in you want the map to start at (always required)
        zoom: 17,
        /*zoomControl: false,*/
        scaleControl: false,
        scrollwheel: false,
        disableDoubleClickZoom: false,

        // The latitude and longitude to center the map (always required)
        center: new google.maps.LatLng(49.8364529, 24.005606), // LNU

        // Style Map
        styles: [{ "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#193341" }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#2c5a71" }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "color": "#29768a" }, { "lightness": -37 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#406d80" }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#406d80" }] }, { "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#3e606f" }, { "weight": 2 }, { "gamma": 0.84 }] }, { "elementType": "labels.text.fill", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "administrative", "elementType": "geometry", "stylers": [{ "weight": 0.6 }, { "color": "#1a3541" }] }, { "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#2c5a71" }] }]
    };

    // Get the HTML DOM element that will contain your map 
    // We are using a div with id="map" seen below in the <body>
    var mapElement = document.getElementById('map');

    // Create the Google Map using our element and options defined above
    var map = new google.maps.Map(mapElement, mapOptions);

    var directionsService = new google.maps.DirectionsService();
    var directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true });;

    directionsDisplay.setMap(map);

    /*-----------------------------------------------------------------------------------------------------------------------------------
     get data from DB
    full_points_info =[{ id,  point{Lat,   Lng}, time   },{ id,  Lat,   Lng, time   }...];
     -----------------------------------------------------------------------------------------------------------------------------------*/
    var full_points_info = [
        { id: "1", point: { Lat: 49.840068, Lng: 24.021885 }, time: "23:23: 23" },
        { id: "2", point: { Lat: 49.837627, Lng: 24.015046 }, time: "23:23: 24" },
        { id: "3", point: { Lat: 49.835625, Lng: 24.008108 }, time: "23:23: 25" },
        { id: "4", point: { Lat: 49.828101, Lng: 23.990172 }, time: "23:23: 26" },
        { id: "5", point: { Lat: 49.813261, Lng: 23.984724 }, time: "23:23: 27" },
        { id: "6", point: { Lat: 49.811499, Lng: 23.989189 }, time: "23:23: 28" }];
    
    var all_points=[];
    for (i = 0; i < full_points_info.length;i++) {
        all_points.push({ location: new google.maps.LatLng(full_points_info[i].point.Lat, full_points_info[i].point.Lng), stopover: true});
    }
    var start = all_points[0];
    var end = all_points[all_points.length - 1];
    var waypts = all_points.slice(1, all_points.length - 1);
    var legs;
    directionsService.route({
        origin: start.location,
        destination: end.location,
        waypoints: waypts,
        optimizeWaypoints: true,
        travelMode: google.maps.TravelMode.DRIVING
    }, function (response, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
            var routes = response.routes[0];
            legs = routes.legs;

            var markerletter = String.fromCharCode("A".charCodeAt(0));
            createMarker(directionsDisplay.getMap(), start, "id: " + full_points_info[0].id,
                legs[0].start_address, markerletter, "red", full_points_info[0]);

            for (i = 0; i < waypts.length; i++) {
                markerletter = String.fromCharCode("A".charCodeAt(0) + i + 1);
                createMarker(directionsDisplay.getMap(), waypts[i], "id: " + full_points_info[i + 1].id,
                    legs[i + 1].start_address, markerletter, "green", full_points_info[i+1]);
            }
            markerletter = String.fromCharCode("A".charCodeAt(0) + waypts.length + 1);
            createMarker(directionsDisplay.getMap(), end, "id: " + full_points_info[full_points_info.length - 1].id,
                legs[legs.length - 1].end_address, markerletter, "red", full_points_info[full_points_info.length-1]);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
        });
    function createMarker(map, latlng, label, html, mark_text, mark_color, info) {
        var contentString = '<b>' + label + '</b><br>' + html;
        var marker = new google.maps.Marker({
            position: latlng.location,
            map: map,
            shadow: iconShadow,
            icon: getMarkerImage(mark_text, mark_color),
            shape: iconShape,
            title: label,
            zIndex: Math.round(latlng.location.lat() * -100000) << 5
        });
        marker.myname = label;
        var fn = function () {
            infowindow.setContent(contentString);
            infowindow.open(map, marker);
        }
        google.maps.event.addListener(marker, 'click', fn);

        //Push data intu table
        var element = document.getElementById("my_tbody");
        var row = element.insertRow(element.rows.length);
        row.onclick = fn;
        var cell1 = row.insertCell(0);
        cell1.innerHTML = info.id;       
        var cell2 = row.insertCell(1);
        cell2.innerHTML = info.point.Lat
        var cell3 = row.insertCell(2);
        cell3.innerHTML = info.point.Lng
        var cell4 = row.insertCell(3);
        cell4.innerHTML = info.time
        return marker;
    }
    
}
