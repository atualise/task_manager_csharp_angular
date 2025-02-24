import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskFormComponent } from './task-form.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskService } from '../../../core/services/task.service';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';

describe('TaskFormComponent', () => {
  let component: TaskFormComponent;
  let fixture: ComponentFixture<TaskFormComponent>;
  let taskService: jasmine.SpyObj<TaskService>;
  let dialogRef: jasmine.SpyObj<MatDialogRef<TaskFormComponent>>;

  beforeEach(async () => {
    const taskServiceSpy = jasmine.createSpyObj('TaskService', ['createTask', 'updateTask']);
    taskServiceSpy.createTask.and.returnValue(of({}));
    taskServiceSpy.updateTask.and.returnValue(of({}));

    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [TaskFormComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: TaskService, useValue: taskServiceSpy },
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: null }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TaskFormComponent);
    component = fixture.componentInstance;
    taskService = TestBed.inject(TaskService) as jasmine.SpyObj<TaskService>;
    dialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<TaskFormComponent>>;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty values', () => {
    expect(component.form.get('title')?.value).toBe('');
    expect(component.form.get('description')?.value).toBe('');
    expect(component.form.get('status')?.value).toBe('Aberto');
    expect(component.form.get('priority')?.value).toBe('Baixa');
  });

  it('should validate required fields', () => {
    const form = component.form;
    expect(form.valid).toBeFalsy();
    
    form.patchValue({
      title: 'Test Task',
      status: 'Aberto',
      priority: 'Baixa'
    });
    
    expect(form.valid).toBeTruthy();
  });

  it('should save new task', () => {
    const taskData = {
      title: 'New Task',
      description: 'Description',
      status: 'Aberto',
      priority: 'Alta'
    };

    component.form.patchValue(taskData);
    component.save();

    expect(taskService.createTask).toHaveBeenCalled();
    expect(dialogRef.close).toHaveBeenCalledWith(true);
  });
}); 