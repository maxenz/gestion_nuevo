
    function initializeMapa() {

         mapOptions = {
            center: new google.maps.LatLng(-10.324885, -59.380171),
            zoom: 3,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
         map = new google.maps.Map(document.getElementById("map_canvas"),
            mapOptions);

    }

   
