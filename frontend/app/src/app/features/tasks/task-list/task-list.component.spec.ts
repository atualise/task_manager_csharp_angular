import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskListComponent } from './task-list.component';
import { TaskService } from '../../../core/services/task.service';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

describe('TaskListComponent', () => {
  let component: TaskListComponent;
  let fixture: ComponentFixture<TaskListComponent>;
  let taskService: jasmine.SpyObj<TaskService>;
  let dialog: jasmine.SpyObj<MatDialog>;

  beforeEach(async () => {
    const taskServiceSpy = jasmine.createSpyObj('TaskService', ['getTasks', 'deleteTask']);
    const dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    dialogSpy.open.and.returnValue({ afterClosed: () => of(true) } as any);

    await TestBed.configureTestingModule({
      declarations: [TaskListComponent],
      imports: [
        MatTableModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        NoopAnimationsModule,
        MatButtonModule
      ],
      providers: [
        { provide: TaskService, useValue: taskServiceSpy },
        { provide: MatDialog, useValue: dialogSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TaskListComponent);
    component = fixture.componentInstance;
    taskService = TestBed.inject(TaskService) as jasmine.SpyObj<TaskService>;
    dialog = TestBed.inject(MatDialog) as jasmine.SpyObj<MatDialog>;

    taskService.getTasks.and.returnValue(of([]));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load tasks on init', () => {
    expect(taskService.getTasks).toHaveBeenCalled();
    expect(component.dataSource.data).toEqual([]);
  });

  it('should apply filters', () => {
    component.filters = {
      status: 'Aberto',
      priority: 'Alta'
    };
    component.applyFilter();
    expect(taskService.getTasks).toHaveBeenCalledWith('Aberto', 'Alta');
  });

  it('should open task form dialog', () => {
    component.openTaskForm();
    expect(dialog.open).toHaveBeenCalled();
  });
}); 