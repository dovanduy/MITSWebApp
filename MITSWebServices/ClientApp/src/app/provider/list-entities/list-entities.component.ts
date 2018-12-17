import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-list-entities',
  templateUrl: './list-entities.component.html',
  styleUrls: ['./list-entities.component.scss']
})
export class ListEntitiesComponent implements OnInit {

  constructor() { }

  @Input() entities;
  @Input() activeEntity;
  @Input() title;
  @Output() edit = new EventEmitter<any>();

  ngOnInit() {

  }

  editEntity(entity) {
    this.activeEntity = entity;
    this.edit.emit(entity);
  }

}
