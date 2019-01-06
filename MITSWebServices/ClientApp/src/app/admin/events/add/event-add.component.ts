import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators
} from "@angular/forms";

import { AllEvents, EventInput } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'event-add',
  templateUrl: './event-add.component.html',
  styleUrls: ['./event-add.component.scss']
})
export class EventAddComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  @Input() showAddForm: boolean;
  
  @Output() onClose = new EventEmitter<boolean>();
  @Output() onAdd = new EventEmitter<EventInput>();

  addEventForm: FormGroup;

  ngOnInit() {
    this.addEventForm = this.fb.group({
      waEventId: ["", [Validators.required, Validators.max(100), Validators.pattern('/^\d+$/')]],
    });
  }

  close() {
    this.showAddForm = false;
    this.onClose.emit(false);
  }

  submitHandler() {
    var newEvent: EventInput =
    {
      mainEventId: this.addEventForm.value.waEventId,
      eventRegistrationType: "main"

    };

    this.onAdd.emit(newEvent);
  }

}
