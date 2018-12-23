import { Component, OnInit, Inject } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import { AllEvents } from "src/app/graphql/generated/graphql";

@Component({
  selector: "register-dialog",
  templateUrl: "./register-dialog.component.html",
  styleUrls: ["./register-dialog.component.scss"],
})
export class RegisterDialogComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<RegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public event: any,
    private _formBuilder: FormBuilder
  ) {}

  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;

  ngOnInit() {

    console.log(this.event);
    console.log(this.event.name);

    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.thirdFormGroup = this._formBuilder.group({
      thirdCtrl: ['', Validators.required]
    });
  }
}
