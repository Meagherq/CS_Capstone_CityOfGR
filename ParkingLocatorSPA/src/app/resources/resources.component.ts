import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html',
  styleUrls: ['./resources.component.scss']
})
export class ResourcesComponent implements OnInit {
  
  public isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
  .pipe(map(result => result.matches));

  constructor( private breakpointObserver: BreakpointObserver) { }

  ngOnInit() {
  }



  goDash() {
    window.location.href='https://www.grandrapidsmi.gov/Residents/Parking-and-Mobility/DASH-the-Downtown-Area-Shuttle';
  }
  goCity() {
    window.location.href='https://www.grandrapidsmi.gov';
  }

  goFacebook() {
    window.location.href='https://www.facebook.com/CityofGrandRapids';
  }
  goYoutube() {
    window.location.href='https://www.youtube.com/user/TheCityofGR';
  }
  goTwitter() {
    window.location.href='https://twitter.com/CityGrandRapids';
  }
  v
  goInstagram() {
    window.location.href='https://www.instagram.com/citygrandrapids/';
  }
}
