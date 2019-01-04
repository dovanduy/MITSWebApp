import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

import { AdminDataService } from '../services/admin-data.service';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { CheckInAttendeeGQL, CheckInAttendeeInput } from 'src/app/graphql/generated/graphql';

import { Result } from '@zxing/library';

@Component({
  selector: 'app-checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.scss']
})
export class CheckinComponent implements OnInit {

  constructor(private adminData: AdminDataService, private checkInAttendeeGQL: CheckInAttendeeGQL) { }

  @ViewChild('scanner')
  scanner: ZXingScannerComponent;
  
  hasDevices: boolean;
  hasPermission: boolean;
  qrResultString: string;
  qrResult: Result;
  scannerEnabled: boolean = true;

  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo;

  checkInForm = new FormGroup({
    registrationId: new FormControl('')
  });

  ngOnInit() {
    this.adminData.pageTitle('Check In');

    if (this.scanner != null) {
      this.scanner.camerasFound.subscribe((devices: MediaDeviceInfo[]) => {
        this.hasDevices = true;
        this.availableDevices = devices;
        this.scanner.changeDevice(devices[0]);
        this.currentDevice = devices[0];
      });
  
      this.scanner.camerasNotFound.subscribe(() => this.hasDevices = false);
      this.scanner.scanComplete.subscribe((result: Result) => this.qrResult = result);
      this.scanner.permissionResponse.subscribe((perm: boolean) => this.hasPermission = perm);
    }

   
  }

  handleQrCodeResult(resultString: string) {
    console.debug('Result: ', resultString);
    this.qrResultString = resultString;
    this.scannerEnabled = false;
    this.CheckIn(parseInt(this.qrResultString));

  }

  displayCameras(cameras: MediaDeviceInfo[]) {
    console.debug('Devices: ', cameras);
    this.availableDevices = cameras;
  }

  onDeviceSelectChange(selectedValue: string) {
    console.debug('Selection changed: ', selectedValue);
    this.currentDevice = this.scanner.getDeviceById(selectedValue);
  }

  CheckIn(registrationId: number) {

    var checkInAttendee: CheckInAttendeeInput = {
      registrationId: registrationId
    }

    this.checkInAttendeeGQL.mutate({
      checkInAttendee: checkInAttendee
    }).subscribe(result => console.log(result));
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value

    this.scannerEnabled = true;
    var id = this.checkInForm.value.registrationId;
    console.log(id);
  }

  stateToEmoji(state: boolean): string {

    const states = {
      // not checked
      undefined: '❔',
      // failed to check
      null: '⭕',
      // success
      true: '✔',
      // can't touch that
      false: '❌'
    };

    return states['' + state];
  }


}
