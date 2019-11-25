import { Observable, Subject } from "rxjs";
import { Injectable } from '@angular/core';

@Injectable()
  export class BreakpointNotifierService {
  public isMobile = false;
  constructor() { }
  
    public thisIsMobile(boo: boolean): void{
        this.isMobile = boo;
        console.log(boo);
    }
  }