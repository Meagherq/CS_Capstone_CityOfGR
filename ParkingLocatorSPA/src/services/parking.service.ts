import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ParkingService {

constructor(private http: HttpClient) { }

getSocrataMasterList() {
    return this.http.get('https://localhost:5000/api/Parking/spaces');
}
}
