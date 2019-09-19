import { Component } from '@angular/core';
import { ParkingService } from 'src/services/parking.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

    title = 'ParkingLocatorSPA';
    value: any[];
    constructor(private parkingService: ParkingService) {
        this.parkingService.getValue().subscribe((x: any[]) => {
            this.value = x;
        });
    }

}
