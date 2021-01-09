import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ageValidator } from '../validators/ageAsyncValidator';
import { privateNumberValidator } from '../validators/pnAsyncValidator';
import { StudentService } from '../_services/student.service';
import { Utils } from '../_services/utils.service';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {
  genderList = null;
  isUpdate = false;
  studentPassedId = 0;
  isLoadingResults = true;

  form = this.formBuilder.group({
    privateNumber: ['', [Validators.required, Validators.pattern("^[0-9]*$")]],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    birthDate: [null, Validators.required, [this.ageValidator.validate.bind(this)]],
    genderId: [null, Validators.required],
    id: 0
  })


  constructor(private formBuilder: FormBuilder,
    private studentService: StudentService,
    private dialogRef: MatDialogRef<StudentComponent>,
    @Inject(MAT_DIALOG_DATA) public passedData: any,
    private utils: Utils,
    private pnValidator: privateNumberValidator,
    private ageValidator: ageValidator,
    private snackBar: MatSnackBar
  ) {
    this.isUpdate = passedData.isUpdate;
    this.studentPassedId = this.passedData.studentId;
  }

  ngOnInit() {

    this.genderList = this.studentService.getGender();
    if (this.isUpdate) {
      this.studentService.getStudent(this.studentPassedId).subscribe(studentData => {
        this.form.patchValue(studentData);
        this.isLoadingResults =false;
      },
        (error) => {
          this.snackBar.open(error.message);
        }
      )
    }


    else {
      //set pn async validator on new record, update should not have this
      this.form.get('privateNumber').setAsyncValidators([this.pnValidator.validate.bind(this)]);
      this.form.get('privateNumber').updateValueAndValidity();
      this.isLoadingResults = false;
    }

  }




  closeDialog = () => {
    let studentInfo = this.form.getRawValue();
    studentInfo.birthDate = this.utils.transformDate(studentInfo.birthDate);
    this.utils.spinner.next(true);  
    if (this.isUpdate) {
      this.studentService.updateStudent(studentInfo).subscribe(() => {
        this.dialogRef.close(true);
        this.snackBar.open('განახლდა წარმატებით');
        
      },
        (error) => {
          this.snackBar.open(error.message)
        }
      )
    }
    else {
      this.studentService.setStudent(studentInfo).subscribe((result) => {
        this.dialogRef.close(true);
        this.snackBar.open('ჩაიწერა წარმატებით');
      },
        (error) => {
          this.snackBar.open(error.message)
        }
      )
    }
  }

}
