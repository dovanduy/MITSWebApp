import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

import { AllSpeakers, SpeakerInput, CreateSpeakerGQL } from '../../../graphql/generated/graphql'

@Component({
  selector: 'speaker-add',
  templateUrl: './speaker-add.component.html',
  styleUrls: ['./speaker-add.component.scss']
})
export class SpeakerAddComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  @Input() speakers: AllSpeakers.Speakers[];
  @Output() onAdd = new EventEmitter<SpeakerInput>();
  showForm: boolean = false;
  addSpeakerForm: FormGroup;

  ngOnInit() {
    this.activate();
  }

  activate() {
    this.addSpeakerForm = this.fb.group({
      firstName: ['', [
        Validators.required,
        Validators.max(100)
      ]],
      lastName: ['', [
        Validators.required,
        Validators.max(100)
      ]],
      title: ['', [
        Validators.required,
        Validators.max(100)
      ]],
      bio: ['', [
        Validators.required,
        Validators.max(1000)
      ]]
    });

  }

  close(): void {
    this.showForm = false;
  }

  showAddSpeakerForm(): void {
    this.showForm = true;
  }

  submitHandler(): void {
    var newSpeaker: SpeakerInput =
    {
      firstName: this.addSpeakerForm.value.firstName,
      lastName: this.addSpeakerForm.value.lastName,
      title: this.addSpeakerForm.value.title,
      bio: this.addSpeakerForm.value.bio

    };
    this.onAdd.emit(newSpeaker);

  }

}
