// Vue.js Timesheet ViewModel
class TimesheetViewModel {
    constructor() {
        this.timesheet = {
            id: this.generateGuid(),
            personId: '',
            from: this.getDefaultFromDate(),
            to: this.getDefaultToDate(),
            components: []
        };
        
        this.newComponent = {
            timeCode: '',
            units: 0,
            from: this.getDefaultFromDate(),
            to: this.getDefaultToDate()
        };
        
        this.isLoading = false;
        this.errorMessage = '';
        this.successMessage = '';
    }
    
    generateGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            const r = Math.random() * 16 | 0;
            const v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    
    getDefaultFromDate() {
        const now = new Date();
        return now.toISOString().slice(0, 16);
    }
    
    getDefaultToDate() {
        const now = new Date();
        now.setHours(now.getHours() + 1);
        return now.toISOString().slice(0, 16);
    }
    
    addComponent() {
        if (!this.newComponent.timeCode || this.newComponent.units <= 0) {
            this.errorMessage = 'Please fill in all component fields correctly.';
            return;
        }
        
        this.timesheet.components.push({
            timeCode: this.newComponent.timeCode,
            units: parseFloat(this.newComponent.units),
            from: this.newComponent.from,
            to: this.newComponent.to
        });
        
        // Reset form
        this.newComponent = {
            timeCode: '',
            units: 0,
            from: this.getDefaultFromDate(),
            to: this.getDefaultToDate()
        };
        
        this.errorMessage = '';
    }
    
    removeComponent(index) {
        this.timesheet.components.splice(index, 1);
    }
    
    async submitTimesheet() {
        if (!this.timesheet.personId) {
            this.errorMessage = 'Person ID is required.';
            return;
        }
        
        if (this.timesheet.components.length === 0) {
            this.errorMessage = 'At least one component is required.';
            return;
        }
        
        this.isLoading = true;
        this.errorMessage = '';
        this.successMessage = '';
        
        try {
            const payload = {
                id: this.timesheet.id,
                personId: this.timesheet.personId,
                from: new Date(this.timesheet.from).toISOString(),
                to: new Date(this.timesheet.to).toISOString(),
                components: this.timesheet.components.map(c => ({
                    timeCode: c.timeCode,
                    units: c.units,
                    from: new Date(c.from).toISOString(),
                    to: new Date(c.to).toISOString()
                }))
            };
            
            const response = await fetch('/api/timesheets', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(payload)
            });
            
            if (response.ok) {
                this.successMessage = 'Timesheet submitted successfully!';
                this.resetForm();
            } else {
                const error = await response.text();
                this.errorMessage = `Failed to submit timesheet: ${error}`;
            }
        } catch (error) {
            this.errorMessage = `Error submitting timesheet: ${error.message}`;
        } finally {
            this.isLoading = false;
        }
    }
    
    resetForm() {
        this.timesheet = {
            id: this.generateGuid(),
            personId: '',
            from: this.getDefaultFromDate(),
            to: this.getDefaultToDate(),
            components: []
        };
        
        this.newComponent = {
            code: '',
            units: 0,
            from: this.getDefaultFromDate(),
            to: this.getDefaultToDate()
        };
    }
    
    calculateTotalUnits() {
        return this.timesheet.components.reduce((sum, component) => sum + component.units, 0);
    }
    
    formatDateTime(dateTimeString) {
        const date = new Date(dateTimeString);
        return date.toLocaleString();
    }
}
