import { Component, Input, EventEmitter, Output, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'edit-entity',
  templateUrl: './edit-entity.component.html',
  styleUrls: ['./edit-entity.component.scss']
})
export class EditEntityComponent implements OnInit {

  constructor() { }

  @Input() entity;
  @Input() Title;
  @Input() editForm: FormGroup;
  @Input() editFormFields;

  @Output() onReset = new EventEmitter<any>();
  @Output() onClose = new EventEmitter<boolean>();
  @Output() onDelete = new EventEmitter<number>();
  @Output() onEdit = new EventEmitter<any>();

  ngOnInit() {
    // console.log(this.entity);
    // console.log(this.editForm);
    // console.log(this.editFormFields);
  }

  close() {
    this.onClose.emit(true);
  }

  delete() {
    this.onDelete.emit(this.entity.id);
    this.close();
  }

  reset() {
    this.onReset.emit(this.entity);
  }

  submitHandler() {
    this.onEdit.emit(this.editForm);
    this.close();
  }


}
