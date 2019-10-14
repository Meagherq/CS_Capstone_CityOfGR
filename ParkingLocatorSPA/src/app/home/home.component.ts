import { Component, OnInit } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';
import { NbSearchService } from '@nebular/theme';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { mapToMapExpression } from '@angular/compiler/src/render3/util';
declare let L: any;

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
              console.log(this.searchResult);
              console.log(this.searchResult[0].boundingbox[0] + ` - ` + this.searchResult[0].boundingbox[2]);
              this.map.flyTo(new L.LatLng(this.searchResult[0].boundingbox[0], this.searchResult[0].boundingbox[2]), 18);
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
