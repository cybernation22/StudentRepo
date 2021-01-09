import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { StudentService } from '../_services/student.service';
import { map } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })
export class privateNumberValidator implements AsyncValidator {
    constructor(private studentService: StudentService) { }

    validate(
        ctrl: AbstractControl
    ): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
        return this.studentService.checkPrivateNumber(ctrl.value)
            .pipe(
                map(res => {

                    if (res) {
                        return { 'pnExists': 'მოცემული პირადი ნომერი უკვე არსებობს' };
                    }
                })
            );
    }
}
