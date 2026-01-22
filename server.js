const express = require('express');
const path = require('path');
const cors = require('cors');

const app = express();
const PORT = 3000;

// Enable CORS for all routes
app.use(cors());

app.use((req, res, next) => {
    console.log(`${req.method} ${req.url}`);
    next();
})

// Serve static files from the public directory
app.use(express.static(path.join(__dirname,'static')));

// Health check endpoint
app.get('/health', (req, res) => {
    res.json({ status: 'ok', message: 'Raidclears server is running' });
});

// Root endpoint
app.get('/', (req, res) => {
    res.json({
        message: 'RaidClears dev Server',
        endpoints: {
            health: '/health'
        }
    });
});

app.listen(PORT, () => {
    console.log(`Clears Tracker static host dev server running on http://localhost:${PORT}`);
    console.log(`Health check at http://localhost:${PORT}/health`);
    console.log('');
    console.log('Press Ctrl+C to stop the server');
}); 