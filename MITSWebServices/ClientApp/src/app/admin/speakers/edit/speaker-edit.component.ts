import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

import { AllSpeakers, UpdateSpeaker } from '../../../graphql/generated/graphql'
import { AdminDataService } from '../../services/admin-data.service';

@Component({
  selector: 'speaker-edit',
  templateUrl: './speaker-edit.component.html',
  styleUrls: ['./speaker-edit.component.scss']
})
export class SpeakerEditComponent implements OnInit, OnChanges {

  constructor(private adminData: AdminDataService, private fb: FormBuilder) { }

  @Input() speaker: AllSpeakers.Speakers;
  @Output() onClose = new EventEmitter<boolean>();
  @Output() onDelete = new EventEmitter<AllSpeakers.Speakers>();
  @Output() onEdit = new EventEmitter<UpdateSpeaker.UpdateSpeaker>();

  editSpeakerForm: FormGroup;

  ngOnInit() {
    this.activate();
  }

  ngOnChanges() {
    this.activate();
  }

  activate() {
    this.editSpeakerForm = this.fb.group({
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

    //Initialize the form
    this.editSpeakerForm.setValue({
      firstName: this.speaker.firstName,
      lastName: this.speaker.lastName,
      title: this.speaker.title,
      bio: this.speaker.bio
    });

  }

  close() {
    this.adminData.removeActiveFromSpeakerList(true);
    this.onClose.emit(true);
  }

  reset() {
    this.editSpeakerForm.reset({
      firstName: this.speaker.firstName,
      lastName: this.speaker.lastName,
      title: this.speaker.title,
      bio: this.speaker.bio
    });
  }

  delete() {
    console.log('It is in the delete');
    this.onDelete.emit(this.speaker);
  }

  submitHandler() {
    console.log('It is in the submit handler');
    var updateSpeaker: UpdateSpeaker.UpdateSpeaker =
    {
      id: this.speaker.id,
      firstName: this.editSpeakerForm.value.firstName,
      lastName: this.editSpeakerForm.value.lastName,
      title: this.editSpeakerForm.value.title,
      bio: this.editSpeakerForm.value.bio

    };
    this.onEdit.emit(updateSpeaker);

  }
  

}
