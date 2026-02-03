import os
from playwright.sync_api import sync_playwright

def verify_game():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        page = browser.new_page()

        # Load the local HTML file
        cwd = os.getcwd()
        file_path = f"file://{cwd}/docs/index.html"
        print(f"Loading {file_path}")
        page.goto(file_path)

        # Screenshot 1: Memorize Phase
        # Wait for grid to be populated
        page.wait_for_selector("#grid .card")
        page.screenshot(path="verification/memorize_phase.png")
        print("Captured memorize_phase.png")

        # Fast forward timer to 1 second
        page.evaluate("timer = 1")

        # Wait for Solve Phase (status text change)
        # The text changes to "Arrange the English words!"
        page.wait_for_function("document.getElementById('status-text').textContent.includes('Arrange')")

        # Screenshot 2: Solve Phase
        page.screenshot(path="verification/solve_phase.png")
        print("Captured solve_phase.png")

        browser.close()

if __name__ == "__main__":
    verify_game()
