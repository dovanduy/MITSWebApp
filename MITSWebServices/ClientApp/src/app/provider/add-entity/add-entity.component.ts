import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup } from "@angular/forms";

@Component({
  selector: "add-entity",
  templateUrl: "./add-entity.component.html",
  styleUrls: ["./add-entity.component.scss"]
})
export class AddEntityComponent {
  constructor() {}

  @Input() addForm: FormGroup;
  @Input() title;
  @Input() formFields;
  @Input() showAddForm: boolean;

  @Output() onAdd = new EventEmitter<any>();
  @Output() onClose = new EventEmitter<Boolean>();

  close(): void {
    this.addForm.reset();
    this.showAddForm = false;
    this.onClose.emit(false);
  }

  submitHandler(): void {
    this.onAdd.emit(this.addForm);
    this.close();
    
  }
}
