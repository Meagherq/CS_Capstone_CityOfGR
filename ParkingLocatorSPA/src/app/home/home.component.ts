import { Component, OnInit, NgModule } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { RouterModule, Router, NavigationEnd, RoutesRecognized } from '@angular/router';
import * as mapboxgl from 'mapbox-gl';
import * as MapboxGeocoder from 'mapbox-gl-geocoder';
import { environment } from 'src/environments/environment';
import { Feature } from 'geojson';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';

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
    availableSpots: any;
    showMap = true;
    title = 'ParkingLocatorSPA';
    value: any[];
    isAvailable = false;
    searchValue: any;
    searchResult: any;
    searchLocation: any;
    map: mapboxgl.Map;
    style = 'mapbox://styles/mapbox/streets-v11';
    lat = 42.9638544;
    lng = -85.6678109;
    greenArray: Array<Feature> = new Array<Feature>();
    redArray: Array<Feature> = new Array<Feature>();
    greyArray: Array<Feature> = new Array<Feature>();

    public links = [
        { displayText: 'Parking', path: '', icon: 'car-hatchback' },
        { displayText: 'Events', path: 'events', icon: 'calendar' },
        { displayText: 'Resources', path: 'resources', icon: 'web' },
    ];

    public isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
        .pipe(map(result => result.matches));


    constructor(private parkingService: ParkingService, private http: HttpClient, private breakpointObserver: BreakpointObserver, private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.router.events.subscribe((data) => {

            if (data instanceof RoutesRecognized) {
            if(data.state.root.firstChild) {
                if(data.state.root.firstChild.data.Map === false) {
                    this.showMap = false;
                  } else {
                      this.showMap = true;
                  }
            } else {
                this.showMap = true;
            }
            if(this.showMap) {
                (mapboxgl as any).accessToken = environment.mapbox.accessToken;
                this.map = new mapboxgl.Map({
                    container: 'map',
                    style: this.style,
                    zoom: 13,
                    center: [this.lng, this.lat]
                });
        
                const geocoder = new MapboxGeocoder({ // Initialize the geocoder
                    accessToken: mapboxgl.accessToken, // Set the access token
                    placeholder: 'Search for Places in Grand Rapids',
                    country: 'US',
                    zoom: 17,
                    bbox: [-85.781, 42.9183, -85.5848, 43.0011],
                    clearOnBlur: true,
                    mapboxgl, // Set the mapbox-gl instance
                    marker: false,
                });
        
                this.map.addControl(geocoder);
                this.map.addControl(new mapboxgl.NavigationControl(), 'bottom-right');
        
                this.mapParkingSpots();
        
                this.map.on('mouseenter', 'greenMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = 'pointer';
                });
                this.map.on('mouseleave', 'greenMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = '';
                });
                this.map.on('mouseenter', 'redMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = 'pointer';
                });
                this.map.on('mouseleave', 'redMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = '';
                });
                this.map.on('mouseenter', 'greyMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = 'pointer';
                });
                this.map.on('mouseleave', 'greyMasterSpaces', () => {
                    this.map.getCanvas().style.cursor = '';
                });
                geocoder.on('result', async (e) => {
                    console.log(e);
                    this.searchLocation = e.result.text;
                    this.availableSpots = 'Calculating';
                    this.isAvailable = true;
                    await this.delay(2200);
                    const element = document.getElementById('map');
                    const positionInfo = element.getBoundingClientRect();
                    const height = positionInfo.height;
                    const width = positionInfo.width;
                    const point = this.map.project(e.result.geometry.coordinates);
                    console.log(e.result.geometry.coordinates[0] + ' : ' + e.result.geometry.coordinates[1]);
                    const bbox: [mapboxgl.PointLike, mapboxgl.PointLike] =
                    [[point.x - (width * .5), point.y - (height * .5)],
                    [point.x + (width * .5), point.y + (height * .5)]];
                    const features = this.map.queryRenderedFeatures(bbox, { layers: ['greyMasterSpaces'] });
                    this.availableSpots = features.length;
                });
        
                // find current location and nav to
                // if (navigator.geolocation) {
                //     navigator.geolocation.getCurrentPosition(position => {
                //      this.lat = position.coords.latitude;
                //      this.lng = position.coords.longitude;
                //      this.map.flyTo({
                //        center: [this.lng, this.lat]
                //      })
                //    });
                // }
        
                this.map.on('load', () => {
                        this.map.resize();
                    });
            }
            }
          });
       
      
        }

    delay(ms: number) {
        return new Promise( resolve => setTimeout(resolve, ms) );
    }

    mapParkingSpots() {
        this.parkingService.getSocrataMasterList().subscribe(data => {
            const socrataMaster: any = data;
            console.log(socrataMaster);
            socrataMaster[0].forEach(greenList => {
                    const greenjson: Feature = {
                        id: greenList.objectId.toString(),
                        type: 'Feature',
                        geometry: {
                            type: 'Polygon',
                            coordinates: [
                                [
                                    [
                                        greenList.boundingBox[0][0], greenList.boundingBox[0][1]
                                    ],
                                    [
                                        greenList.boundingBox[1][0], greenList.boundingBox[1][1]
                                    ],
                                    [
                                        greenList.boundingBox[2][0], greenList.boundingBox[2][1]
                                    ],
                                    [
                                        greenList.boundingBox[3][0], greenList.boundingBox[3][1]
                                    ]
                                ],
                            ],
                        },
                        properties: {
                        }
                    };
                    this.greenArray.push(greenjson);
                });
            socrataMaster[1].forEach(redList => {
                    const redjson: Feature = {
                        id: redList.objectId.toString(),
                        type: 'Feature',
                        geometry: {
                            type: 'Polygon',
                            coordinates: [
                                [
                                    [
                                        redList.boundingBox[0][0], redList.boundingBox[0][1]
                                    ],
                                    [
                                        redList.boundingBox[1][0], redList.boundingBox[1][1]
                                    ],
                                    [
                                        redList.boundingBox[2][0], redList.boundingBox[2][1]
                                    ],
                                    [
                                        redList.boundingBox[3][0], redList.boundingBox[3][1]
                                    ]
                                ],
                            ],
                        },
                        properties: {
                        }
                    };
                    this.redArray.push(redjson);
                });
            socrataMaster[2].forEach(greyList => {
                    const greyjson: Feature = {
                        id: greyList.objectId.toString(),
                        type: 'Feature',
                        geometry: {
                            type: 'Polygon',
                            coordinates: [
                                [
                                    [
                                        greyList.boundingBox[0][0], greyList.boundingBox[0][1]
                                    ],
                                    [
                                        greyList.boundingBox[1][0], greyList.boundingBox[1][1]
                                    ],
                                    [
                                        greyList.boundingBox[2][0], greyList.boundingBox[2][1]
                                    ],
                                    [
                                        greyList.boundingBox[3][0], greyList.boundingBox[3][1]
                                    ]
                                ],
                            ],
                        },
                        properties: {
                        }
                    };
                    this.greyArray.push(greyjson);
                });
            this.map.addSource('greengrspaces', {
                type: 'geojson',
                data: {
                    type: 'FeatureCollection',
                    features: this.greenArray
                },
            });
            this.map.addSource('redgrspaces', {
                type: 'geojson',
                data: {
                    type: 'FeatureCollection',
                    features: this.redArray
                },
            });
            this.map.addSource('greygrspaces', {
                type: 'geojson',
                data: {
                    type: 'FeatureCollection',
                    features: this.greyArray
                },
            });
            this.map.addLayer({
                id: 'greenMasterSpaces',
                type: 'fill',
                source: 'greengrspaces',
                minzoom: 15,
                paint: {
                    'fill-color': '#00cc00',
                    'fill-outline-color': '#000000',
                },
            });
            this.map.addLayer({
                id: 'redMasterSpaces',
                type: 'fill',
                source: 'redgrspaces',
                minzoom: 15,
                paint: {
                    'fill-color': '#f03b20',
                    'fill-outline-color': '#000000',
                },
            });
            this.map.addLayer({
                id: 'greyMasterSpaces',
                type: 'fill',
                source: 'greygrspaces',
                minzoom: 15,
                paint: {
                    'fill-color': '#808080',
                    'fill-outline-color': '#000000',
                },
            });

        });
    }
}
