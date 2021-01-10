import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { searchCriteria } from '../models/searchCriteria';
import { studentModel } from '../models/student';
import { Utils } from './utils.service';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  constructor(private httpClnt: HttpClient,
    private utilService: Utils) { }


  getGender() {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetGender`)
      .pipe(catchError(this.handleError));
  }
  
  getStudents(criteria: searchCriteria) {
    return this.httpClnt.get<studentModel[]>(`${environment.apiUrl}/Student/Getstudents`,
      {
        params: new HttpParams()
          .set('privateNumber', criteria.privateNumber || '')
          .set('birthDateFrom', criteria.birthDateFrom? criteria.birthDateFrom.toString() : '')
          .set('birthDateTo', criteria.birthDateTo ? criteria.birthDateTo.toString() : '')
          .set('page', criteria.page ? criteria.page.toString() : '0')
          .set('pageSize', criteria.pageSize.toString())
      }
    )
      .pipe(catchError(this.handleError));
  }

  setStudent(student) {
    return this.httpClnt.post(`${environment.apiUrl}/Student/PostStudent`, student)
      .pipe(catchError(this.handleError));
  }


  updateStudent(student) {
    return this.httpClnt.put(`${environment.apiUrl}/Student/ModifyStudent`,
      student
    ).pipe(catchError(this.handleError));
  }

  removeStudent(id) {
    return this.httpClnt.delete(`${environment.apiUrl}/Student/DeleteStudent/${id}`)
      .pipe(catchError(this.handleError));
  }

  checkPrivateNumber(pn) {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetstudentByPn?privateNumber=${pn}`)
      .pipe(catchError(this.handleError));
  }

  getStudent(id) {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetStudent?id=${id}`)
      .pipe(catchError(this.handleError));
  }

  handleError(errorResponse: HttpErrorResponse) {
    debugger;
    if (errorResponse.error instanceof ProgressEvent) {
      return throwError({
        isServer: false,
        message: errorResponse.message
      });
    }
    else {

      return throwError({
        isServer: true,
        message: errorResponse.error
      });
    }

  }

  ageValidator(birthdate) {
    let convertedDate = this.utilService.transformDate(birthdate).toString();

    return this.httpClnt.get(`${environment.apiUrl}/Student/AgeValidator?birthDate=${convertedDate}`)
      .pipe(catchError(this.handleError));
  }



}
