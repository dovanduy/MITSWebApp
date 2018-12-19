import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'event-add',
  templateUrl: './event-add.component.html',
  styleUrls: ['./event-add.component.scss']
})
export class EventAddComponent implements OnInit {

  constructor() { }

  @Input() showAddForm: boolean;
  
  @Output() onClose = new EventEmitter<boolean>();

  ngOnInit() {
  }

  close() {
    this.showAddForm = false;
    this.onClose.emit(false);
  }

}
