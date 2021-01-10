import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { StudentService } from 'src/app/_services/student.service';
import { pagingModel } from '../models/paging';
import { searchCriteria } from '../models/searchCriteria';
import { studentModel } from '../models/student';
import { StudentComponent } from '../student/student.component';
import { Utils } from '../_services/utils.service';

@Component({
  selector: 'app-studentsList',
  templateUrl: './studentsList.component.html',
  styleUrls: ['./studentsList.component.css']
})
export class StudentsListComponent implements OnInit {
  paging = new pagingModel;
  isLoadingResults = true;

  displayedColumns: string[] = ['privateNumber', 'firstName', 'lastName', 'birthDate', 'sex', 'icons'];
  dataSource: MatTableDataSource<studentModel>;

  form = this.formBuilder.group({
    privateNumber: ['', [Validators.pattern("^[0-9]*$")]],
    birthDateFrom: null,
    birthDateTo: null
  })

  constructor(private studentService: StudentService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private utils: Utils,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.getStudentsList();
    this.makeSpinnerSpin();
  }



  clearCriteria = () => {
    this.form.reset();
    this.getStudentsList();
  }

  makeSpinnerSpin() {
    this.utils.spinner.subscribe((visibility: boolean) => {
      this.isLoadingResults = visibility;
    })
  }

  searchByCriteria = () => {
    let formValue = this.form.getRawValue();

    let criteria =
    {
      ...formValue,
      birthDateFrom: formValue.birthDateFrom ? this.utils.transformDate(formValue.birthDateFrom) : null,
      birthDateTo: formValue.birthDateTo ? this.utils.transformDate(formValue.birthDateTo) : null,
    }

    this.getStudentsList(criteria);
  }

  deleteItem = (item: studentModel) => {
    this.studentService.removeStudent(item.id).subscribe(res => {
      this.getStudentsList();
      this.snackBar.open("წაიშალა წარმატებით");
    },
      (error) => {
        this.snackBar.open(error.message)
      }
    )
  }


  openDialog(id?: number) {
    let isUpdate = id ? true : false;
    const dialogRef = this.dialog.open(StudentComponent, {
      width: '400px',
      height: 'auto',
      data: {
        isUpdate: isUpdate,
        studentId: id
      }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getStudentsList();
      }
    });

  }

  getStudentsList(criteria?: searchCriteria, event?: any) {
    this.utils.spinner.next(true);

    let searchTerm = {
      ...criteria,
      page: event ? event.pageIndex : 0,
      pageSize: event ? event.pageSize : this.paging.pageSize
    }

    this.studentService.getStudents(searchTerm)
      .subscribe((serverData: any) => {
        this.paging = serverData.paginator;
        this.dataSource = new MatTableDataSource(serverData.students);

      },
        (error) => {
          this.snackBar.open(error.message)
        },
        () => {
          this.utils.spinner.next(false);
        }
      )
  }



}
