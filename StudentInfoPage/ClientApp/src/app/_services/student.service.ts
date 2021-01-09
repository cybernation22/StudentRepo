import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { searchCriteria } from '../models/searchCriteria';
import { studentModel } from '../models/student';
import { Utils } from './utils.service';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  constructor(private httpClnt: HttpClient,
    private utilService: Utils) {}

    
  getGender() {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetGender`);
  }

  getStudents(criteria: searchCriteria) {
    return this.httpClnt.post<studentModel[]>(`${environment.apiUrl}/Student/Getstudents`,
      criteria
    );
  }

  setStudent(student) {
    return this.httpClnt.post(`${environment.apiUrl}/Student/PostStudent`, student);
  }

   
  updateStudent(student) {
    return this.httpClnt.put(`${environment.apiUrl}/Student/ModifyStudent`,
     student
    );
  }

  removeStudent(id) {
    return this.httpClnt.delete(`${environment.apiUrl}/Student/DeleteStudent/${id}`)
  }

  checkPrivateNumber(pn) {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetstudentByPn?privateNumber=${pn}`);
  }

  getStudent(id) {
    return this.httpClnt.get(`${environment.apiUrl}/Student/GetStudent?id=${id}`);
  }

  ageValidator(birthdate) {
    let convertedDate = this.utilService.transformDate(birthdate).toString();

    return this.httpClnt.get(`${environment.apiUrl}/Student/AgeValidator?birthDate=${convertedDate}`);
  }



}
