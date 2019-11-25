import { Component, OnInit, NgModule } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import * as mapboxgl from 'mapbox-gl';
import * as MapboxGeocoder from 'mapbox-gl-geocoder';
import { environment } from 'src/environments/environment';
import { Feature } from 'geojson';
import { BreakpointNotifierService } from 'src/services/breakpoint-notifier.service';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

declare let L: any;
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

    @NgModule({
        imports: [
            RouterModule
        ],
        declarations: [HomeComponent]
    })
export class HomeComponent implements OnInit {

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type':  'application/json',
        }),
    };
    availableSpots: any = 0;
    title = 'ParkingLocatorSPA';
    value: any[];
    searchValue: any;
    searchResult: any;
    map: mapboxgl.Map;
    style = 'mapbox://styles/mapbox/streets-v11';
    lat = 42.9638544;
    lng = -85.6678109;
    geoArray: Array<Feature> = new Array<Feature>();



    public links = [
        { displayText: 'Parking', path: 'parking', icon: 'car-hatchback' },
        { displayText: 'Events', path: 'events', icon: 'calendar' },
        { displayText: 'Resources', path: 'resources', icon: 'web' },
      ];

      
      public isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
      .pipe(
        map(result => result.matches)
      )


    constructor(private parkingService: ParkingService,private breakpointNotifierService: BreakpointNotifierService, private http: HttpClient, private breakpointObserver: BreakpointObserver) {
       
    }

    ngOnInit() {

        (mapboxgl as any).accessToken = environment.mapbox.accessToken;
        this.map = new mapboxgl.Map({
          container: 'map',
          style: this.style,
          zoom: 13,
          center: [this.lng, this.lat]
        });

        const geocoder = new MapboxGeocoder({ // Initialize the geocoder
            accessToken: mapboxgl.accessToken, // Set the access token
            placeholder: 'Search for places in Grand Rapids',
            country: 'US',
            bbox: [-85.781, 42.9183, -85.5848, 43.0011],
            clearOnBlur: true,
            clearAndBlurOnEsc: true,
            mapboxgl, // Set the mapbox-gl instance
            marker: false,
        });

        this.map.addControl(geocoder);
        this.map.addControl(new mapboxgl.NavigationControl(), 'bottom-right');

        this.mapParkingSpots();

        this.map.on('mouseenter', 'masterSpaces', () => {
            this.map.getCanvas().style.cursor = 'pointer';
            });

        this.map.on('mouseleave', 'masterSpaces', () => {
            this.map.getCanvas().style.cursor = '';
            });

        this.map.on('load', () => {
                this.map.resize();
            });

        }

    mapParkingSpots() {
        this.parkingService.getSocrataMasterList().subscribe(data => {
            const socrataMaster: any = data;
            console.log(socrataMaster);
            socrataMaster.forEach(element => {
                const geojson: Feature = {
                    id: element.objectId.toString(),
                    type: 'Feature',
                    geometry: {
                        type: 'Polygon',
                        coordinates: [
                            [
                                [
                                    element.boundingBox[0][0], element.boundingBox[0][1]
                                ],
                                [
                                    element.boundingBox[1][0], element.boundingBox[1][1]
                                ],
                                [
                                    element.boundingBox[2][0], element.boundingBox[2][1]
                                ],
                                [
                                    element.boundingBox[3][0], element.boundingBox[3][1]
                                ]
                            ],

                        ],
                    },
                    properties: {
                    }
                };
                this.geoArray.push(geojson);
            });
            this.map.addSource('grspaces', {
                type: 'geojson',
                data: {
                    type: 'FeatureCollection',
                    features: this.geoArray
                },
            });
            this.map.addLayer({
                id: 'masterSpaces',
                type: 'fill',
                source: 'grspaces',
                minzoom: 15,
                paint: {
                    'fill-color': '#00cc00',
                    'fill-outline-color': '#000000',
                },
            });
          });
    }
}
