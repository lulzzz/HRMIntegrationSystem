import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ExternalSystem } from '../../models/external-system';
import { Alert } from '../../../core/models/alert';
import { IntegrationCategory } from '../../models/integration-category';
import { Router } from '@angular/router';
import { ConstantsRoutes } from '../../shared/constants/routes-constants';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { Integration, IntegrationService } from '@sticos/apis/integrations';
import { Unit } from '@sticos/apis/common';
import { ExternalSystemsService } from '../../services';
import { IntegrationCategoryService } from '../../services';
import { UnitCacheService, UserCacheService } from 'src/app/core/services';
import { AlertService } from 'src/app/core/services/';
import { TimeoutConstants } from 'src/app/core/constants';
import { AppConfig } from '../../../core/services/app-config-service';

@Component({
  selector: 'app-integration-add',
  templateUrl: './integration-add.component.html',
  styleUrls: ['./integration-add.component.scss'],
})
export class IntegrationAddComponent implements OnInit, OnDestroy {
  translationSubscription: Subscription;
  messages: any;
  public integrationAddForm: FormGroup;
  alert: Alert;
  addedIntegration: Integration;
  units: Unit[] = [];
  categories: IntegrationCategory[];
  public externalSystems: ExternalSystem[];

  // Wizzard
  public wizardStep = 1;
  public totalSteps = 4;
  public progress = '0';
  customerId: string;
  public stepsCompleted = [];
  public canContinue = false;
  public selectedUnit: Unit[];
  public selectedCategory: number;
  public showOrganizationNumberLink: boolean;

  constructor(
    private fb: FormBuilder,
    private integrationService: IntegrationService,
    private externalSystemService: ExternalSystemsService,
    private alertService: AlertService,
    private unitCacheService: UnitCacheService,
    private categoryService: IntegrationCategoryService,
    private translateService: TranslateService,
    private router: Router,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
    this.translationSubscription = this.translateService.onLangChange.subscribe(
      () => {
        this.fetchMessages();
      },
    );
  }

  ngOnInit() {
    this.onGetUnits();
    this.onGetCategories();
    this.createForm();
    this.fetchMessages();
    this.showOrganizationNumberLink = false;
  }

  onGetUnitSelect(event) {
    this.selectedUnit = this.units.filter(x => x.id === event);
    this.showOrganizationNumberLink =
      this.selectedUnit[0].businessOrganizationNumber === '';
  }

  onGetCategorySelect(event) {
    this.integrationAddForm.controls['category'].setValue(event.value.value);

    this.selectedCategory = parseInt(event.value.value, 10);
    this.onGetExternalSystems();
  }

  onGetExternalSystemSelect(event) {
    this.integrationAddForm.controls['externalSystem'].setValue(
      event.value.value,
    );
  }

  fetchMessages() {
    this.translateService
      .get(['messages.integrations.added-successfully'])
      .subscribe(messages => {
        this.messages = messages;
      });
  }

  // Navigate back
  previousPage(step) {
    if (step >= 1) {
      this.wizardStep--;
      this.calculateProgress();
    }
    this.canContinue = this.stepsCompleted.includes(this.wizardStep);
  }

  // Navigate forward
  nextPage(step) {
    if (step < this.totalSteps) {
      this.wizardStep++;
      this.calculateProgress();
    }
    this.canContinue = this.stepsCompleted.includes(this.wizardStep);
  }

  // Set step
  setStep(step) {
    this.wizardStep = step;
    this.calculateProgress();
    this.canContinue = this.stepsCompleted.includes(this.wizardStep);
  }

  // Set current progress
  calculateProgress() {
    const calculateProgress = (this.wizardStep / this.totalSteps) * 100;
    this.progress = calculateProgress + '%';
  }

  // Check if a step is completed
  isStepCompleted(step) {
    if (step < this.wizardStep) {
      return true;
    } else {
      return false;
    }
  }

  // Check if current step
  isCurrentStep(step) {
    if (step === this.wizardStep) {
      return true;
    } else {
      return false;
    }
  }

  // Set status classes for steps in the overview
  setStatusClass(step) {
    const classes = {
      wizardStepCompleted: this.isStepCompleted(step),
      wizardStepActive: this.isCurrentStep(step),
    };

    return classes;
  }

  createForm() {
    this.integrationAddForm = this.fb.group({
      id: 0,
      unitId: ['', Validators.required],
      category: ['', Validators.required],
      externalSystem: ['', Validators.required],
      isActivated: true,
    });
  }

  onGetExternalSystems() {
    this.externalSystems = null;
    this.integrationAddForm.controls['externalSystem'].setValue('');

    this.externalSystemService
      .onGetExternalSystems(this.selectedCategory)
      .subscribe(data => (this.externalSystems = data));
  }

  onGetUnits(): any {
    this.unitCacheService
      .GetUnits({ customerId: this.customerId })
      .subscribe(data => (this.units = data));
  }

  onGetCategories() {
    this.categoryService
      .getCategories()
      .subscribe(data => (this.categories = data));
  }

  onSubmitAddIntegration() {
    const integrationToSave = this.integrationAddForm.value;
    this.integrationService
      .Create({ integration: integrationToSave, customerId: this.customerId })
      .subscribe(
        data => {
          this.addedIntegration = data;
          this.alert = this.alertService.success(
            this.messages['messages.integrations.added-successfully'],
          );
          this.stepsCompleted = [];
          setTimeout(() => {
            this.alert = null;
            this.router.navigate([ConstantsRoutes.Admin.integrations]);
          }, TimeoutConstants.SuccessTimeout);
        },
        err => {
          this.alert = this.alertService.danger(err.error);
          setTimeout(() => {
            this.alert = null;
          }, TimeoutConstants.ErrorTimeout);
        },
      );
  }

  getCircularReplacer = () => {
    const seen = new WeakSet();
    return (key, value) => {
      if (typeof value === 'object' && value !== null) {
        if (seen.has(value)) {
          return;
        }
        seen.add(value);
      }
      return value;
    };
  };

  stepsConfirmed(event) {
    if (event.value !== null) {
      if (!this.stepsCompleted.includes(this.wizardStep)) {
      }
      this.canContinue = true;
    } else {
      this.canContinue = false;
    }
  }

  ngOnDestroy() {
    this.translationSubscription.unsubscribe();
  }

  onNavigate() {
    window.open(AppConfig.settings.personalUnitListUrl);
  }
}
