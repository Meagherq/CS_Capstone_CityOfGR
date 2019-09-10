import { Component } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ParkingLocatorSPA';
  value: any;
  constructor(private parkingService: ParkingService) {
      this.value = this.parkingService.getValue().subscribe(x => x);
  }
}
