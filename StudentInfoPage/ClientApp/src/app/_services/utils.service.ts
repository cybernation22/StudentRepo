import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Utils {

  constructor(private datePipe: DatePipe) {}

  spinner =  new Subject<boolean>();
  spinnerDialog =  new Subject<boolean>();


  transformDate(date) {
    return this.datePipe.transform(date, 'yyyy-MM-dd');
  }


  kservice(date) {
    return this.datePipe.transform(date, 'yyyy-MM-dd');
  }

}
