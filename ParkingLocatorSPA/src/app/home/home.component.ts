import { Component, OnInit } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';
import { NbSearchService } from '@nebular/theme';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { mapToMapExpression } from '@angular/compiler/src/render3/util';

declare let L: any;
var parkingspot = 
[[
    42.966133652813,
    -85.670216067712 
],
[
    42.966133652813,
    -85.670215097108
],
[
    42.966078458585,
    -85.670248719741
   
],
[
    42.966078458585,
    -85.670215097108
]];

var parkingspot2 = [[
    42.961997937653,
    -85.672433426147
    ],
    [
    42.961997414251,
    -85.672467042291
    ],
    [
    42.962052280431,
    -85.672468625172
    ],
    [
    42.962052803832,
    -85.672435008999
    ],
    [
    42.961997937653,
    -85.672433426147
    ]];

    var parkingspot3 =[[
        42.961770755288,
        -85.666767464271
        ],
        [
        42.961770103943,
        -85.666801076388
        ],
        [
        42.96182496394,
        -85.666803047612
        ],
        [
        42.961825615286,
        -85.666769435465
        ],
        [
         42.961770755288,
        -85.666767464271
        ]];
        var parkingspot4 = [[
            42.9756814769,
            -85.672714419956

            ],
            [
             42.975656961224,
            -85.672718469417

            ],
            [
            42.975663569971,
            -85.672792662621

            ],
            [
             42.975688085663,
            -85.672788611964

            ],
            [
            42.9756814769,
            -85.672714419956

            ]];
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    httpOptions = {
        headers: new HttpHeaders({
          'Content-Type':  'application/json',
        }),
    };

    title = 'ParkingLocatorSPA';
    value: any[];
    searchValue = '';
    searchResult: any;
    map: any;
    constructor(private parkingService: ParkingService, private searchService: NbSearchService, private http: HttpClient) {
        this.parkingService.getValue().subscribe((x: any[]) => {
            this.value = x;
        });

        this.searchService.onSearchSubmit()
        .subscribe((data: any) => {
          this.searchValue = data.term + ` Grand Rapids Michigan`;
          console.log(this.searchValue);
          this.searchOSM().subscribe(x => {
              this.searchResult = x;

              if(data.term = "mySpot") {
                var polygon = L.polygon(parkingspot);
                var polygon2 = L.polygon(parkingspot2);
                var polygon3 = L.polygon(parkingspot3);
                var polygon4 = L.polygon(parkingspot4);
                polygon.addTo(this.map);
                polygon2.addTo(this.map);
                polygon3.addTo(this.map);
                polygon4.addTo(this.map);
                polygon.setStyle({fillColor: '#dddddd'});
                this.map.flyTo(new L.LatLng(    42.961997414251,
                    -85.672467042291
                    ), 18);
              } else {
                console.log(this.searchResult);
                console.log(this.searchResult[0].boundingbox[0] + ` - ` + this.searchResult[0].boundingbox[2]);
              this.map.flyTo(new L.LatLng(this.searchResult[0].boundingbox[0], this.searchResult[0].boundingbox[2]), 18);
              }
            });
        });
    }

    ngOnInit() {
        this.map = L.map('map', {scrollWheelZoom: false}).setView(new L.LatLng(42.9638544, -85.6678109), 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(this.map);

    }
    searchOSM() {
        return this.http.get(`https://nominatim.openstreetmap.org/?format=json&addressdetails=1&q=${this.searchValue}&format=json&limit=1`
        , this.httpOptions);
    }
}
