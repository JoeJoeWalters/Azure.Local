// Initialize Vue.js Timesheet App
const { createApp } = Vue;

const TimesheetApp = {
    data() {
        return new TimesheetViewModel();
    },
    computed: {
        totalUnits() {
            return this.calculateTotalUnits();
        }
    },
    methods: {
        addComponent() {
            TimesheetViewModel.prototype.addComponent.call(this);
        },
        removeComponent(index) {
            TimesheetViewModel.prototype.removeComponent.call(this, index);
        },
        submitTimesheet() {
            TimesheetViewModel.prototype.submitTimesheet.call(this);
        },
        resetForm() {
            TimesheetViewModel.prototype.resetForm.call(this);
        },
        calculateTotalUnits() {
            return TimesheetViewModel.prototype.calculateTotalUnits.call(this);
        },
        formatDateTime(dateTimeString) {
            return TimesheetViewModel.prototype.formatDateTime.call(this, dateTimeString);
        }
    },
    template: `
        <div class="container-fluid timesheet-container">
            <h1 class="mb-4">Timesheet Entry</h1>
            
            <!-- Alert Messages -->
            <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show" role="alert">
                {{ errorMessage }}
                <button type="button" class="btn-close" @click="errorMessage = ''" aria-label="Close"></button>
            </div>
            
            <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
                {{ successMessage }}
                <button type="button" class="btn-close" @click="successMessage = ''" aria-label="Close"></button>
            </div>
            
            <!-- Main Timesheet Form -->
            <div class="card mb-4">
                <div class="card-header">
                    <h5 class="mb-0">Timesheet Information</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label for="personId" class="form-label">Person ID</label>
                            <input 
                                type="text" 
                                class="form-control" 
                                id="personId" 
                                v-model="timesheet.personId"
                                placeholder="Enter person ID"
                                required>
                        </div>
                        <div class="col-md-6">
                            <label for="timesheetId" class="form-label">Timesheet ID</label>
                            <input 
                                type="text" 
                                class="form-control" 
                                id="timesheetId" 
                                v-model="timesheet.id"
                                readonly>
                        </div>
                        <div class="col-md-6">
                            <label for="fromDate" class="form-label">From</label>
                            <input 
                                type="datetime-local" 
                                class="form-control" 
                                id="fromDate" 
                                v-model="timesheet.from"
                                required>
                        </div>
                        <div class="col-md-6">
                            <label for="toDate" class="form-label">To</label>
                            <input 
                                type="datetime-local" 
                                class="form-control" 
                                id="toDate" 
                                v-model="timesheet.to"
                                required>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Add Component Section -->
            <div class="card mb-4">
                <div class="card-header">
                    <h5 class="mb-0">Add Timesheet Component</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-3">
                            <label for="componentTimeCode" class="form-label">Time Code</label>
                            <input 
                                type="text" 
                                class="form-control" 
                                id="componentTimeCode" 
                                v-model="newComponent.timeCode"
                                placeholder="e.g., DEV, MEETING">
                        </div>
                        <div class="col-md-3">
                            <label for="componentProjectCode" class="form-label">Project Code</label>
                            <input
                                type="text"
                                class="form-control"
                                id="componentProjectCode"
                                v-model="newComponent.projectCode"
                                placeholder="e.g., DEV, MEETING">
                        </div>
                        <div class="col-md-3">
                            <label for="componentUnits" class="form-label">Units (Hours)</label>
                            <input 
                                type="number" 
                                class="form-control" 
                                id="componentUnits" 
                                v-model.number="newComponent.units"
                                min="0"
                                step="0.5"
                                placeholder="0.0">
                        </div>
                        <div class="col-md-3">
                            <label for="componentFrom" class="form-label">From</label>
                            <input 
                                type="datetime-local" 
                                class="form-control" 
                                id="componentFrom" 
                                v-model="newComponent.from">
                        </div>
                        <div class="col-md-3">
                            <label for="componentTo" class="form-label">To</label>
                            <input 
                                type="datetime-local" 
                                class="form-control" 
                                id="componentTo" 
                                v-model="newComponent.to">
                        </div>
                    </div>
                    <div class="mt-3">
                        <button 
                            type="button" 
                            class="btn btn-primary" 
                            @click="addComponent">
                            <i class="bi bi-plus-circle"></i> Add Component
                        </button>
                    </div>
                </div>
            </div>
            
            <!-- Components List -->
            <div class="card mb-4" v-if="timesheet.components.length > 0">
                <div class="card-header">
                    <h5 class="mb-0">Components (Total: {{ totalUnits }} hours)</h5>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-hover">
                            <thead>
                                <tr>
                                    <th>Time Code</th>
                                    <th>Project Code</th>
                                    <th>Units</th>
                                    <th>From</th>
                                    <th>To</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(component, index) in timesheet.components" :key="index">
                                    <td>{{ component.timeCode }}</td>
                                    <td>{{ component.projectCode }}</td>
                                    <td>{{ component.units }}</td>
                                    <td>{{ formatDateTime(component.from) }}</td>
                                    <td>{{ formatDateTime(component.to) }}</td>
                                    <td>
                                        <button 
                                            type="button" 
                                            class="btn btn-sm btn-danger" 
                                            @click="removeComponent(index)">
                                            <i class="bi bi-trash"></i> Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            
            <!-- Action Buttons -->
            <div class="d-flex gap-2 mb-4">
                <button 
                    type="button" 
                    class="btn btn-success btn-lg" 
                    @click="submitTimesheet"
                    :disabled="isLoading">
                    <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    <i v-else class="bi bi-check-circle me-2"></i>
                    {{ isLoading ? 'Submitting...' : 'Submit Timesheet' }}
                </button>
                <button 
                    type="button" 
                    class="btn btn-secondary btn-lg" 
                    @click="resetForm"
                    :disabled="isLoading">
                    <i class="bi bi-arrow-counterclockwise me-2"></i>
                    Reset Form
                </button>
            </div>
        </div>
    `
};

// Initialize the app when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    const timesheetElement = document.getElementById('timesheet-app');
    if (timesheetElement) {
        createApp(TimesheetApp).mount('#timesheet-app');
    }
});
